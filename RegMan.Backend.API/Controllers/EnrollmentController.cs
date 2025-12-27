using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegMan.Backend.API.Common;
using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.BusinessLayer.DTOs.EnrollmentDTOs;
using RegMan.Backend.DAL.Contracts;
using RegMan.Backend.DAL.Entities;
using System.Security.Claims;

namespace RegMan.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService enrollmentService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITranscriptService transcriptService;

        public EnrollmentController(IEnrollmentService enrollmentService, IUnitOfWork unitOfWork, ITranscriptService transcriptService)
        {
            this.enrollmentService = enrollmentService;
            this.unitOfWork = unitOfWork;
            this.transcriptService = transcriptService;
        }

        private async Task SyncTranscriptFromEnrollmentAsync(Enrollment enrollment)
        {
            if (enrollment.Section == null)
                enrollment = await unitOfWork.Enrollments
                    .GetAllAsQueryable()
                    .Include(e => e.Section!)
                        .ThenInclude(s => s.Course)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == enrollment.EnrollmentId)
                    ?? enrollment;

            if (enrollment.Section == null || enrollment.Section.Course == null)
                return;

            var course = enrollment.Section.Course;
            var semester = enrollment.Section.Semester ?? string.Empty;
            var year = enrollment.Section.Year.Year;

            var existingTranscript = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .FirstOrDefaultAsync(t =>
                    t.StudentId == enrollment.StudentId &&
                    t.SectionId == enrollment.SectionId &&
                    t.CourseId == course.CourseId);

            var shouldHaveTranscript = enrollment.Status == Status.Completed &&
                !string.IsNullOrWhiteSpace(enrollment.Grade) &&
                GradeHelper.IsValidGrade(enrollment.Grade);

            if (shouldHaveTranscript)
            {
                var grade = enrollment.Grade!.ToUpperInvariant();
                var gradePoints = GradeHelper.GetGradePoints(grade);

                if (existingTranscript == null)
                {
                    existingTranscript = new Transcript
                    {
                        StudentId = enrollment.StudentId,
                        CourseId = course.CourseId,
                        SectionId = enrollment.SectionId,
                        Grade = grade,
                        GradePoints = gradePoints,
                        Semester = semester,
                        Year = year,
                        CreditHours = course.CreditHours,
                        CompletedAt = DateTime.UtcNow
                    };
                    await unitOfWork.Transcripts.AddAsync(existingTranscript);
                }
                else
                {
                    existingTranscript.Grade = grade;
                    existingTranscript.GradePoints = gradePoints;
                    existingTranscript.Semester = semester;
                    existingTranscript.Year = year;
                    existingTranscript.CreditHours = course.CreditHours;
                    existingTranscript.CompletedAt = DateTime.UtcNow;
                    unitOfWork.Transcripts.Update(existingTranscript);
                }
            }
            else
            {
                // Enrollment not completed/graded -> ensure it does not appear as a completed transcript record.
                if (existingTranscript != null)
                {
                    await unitOfWork.Transcripts.DeleteAsync(existingTranscript.TranscriptId);
                }
            }

            await unitOfWork.SaveChangesAsync();
            await transcriptService.RecalculateAndUpdateStudentGPAAsync(enrollment.StudentId);
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User ID claim is missing from the token.");
            return userId;
        }

        // =========================
        // Get Enrollment by ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .Include(e => e.Section)
                    .ThenInclude(s => s!.Course)
                .Include(e => e.Section)
                    .ThenInclude(s => s!.Instructor)
                        .ThenInclude(i => i!.User)
                .Include(e => e.Student)
                    .ThenInclude(s => s!.User)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            // Check authorization - admin can see all, students can only see their own
            var userId = GetUserId();
            if (!User.IsInRole("Admin") && enrollment.Student?.UserId != userId)
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    ApiResponse<string>.FailureResponse("Forbidden", StatusCodes.Status403Forbidden));

            var dto = new ViewEnrollmentDTO
            {
                EnrollmentId = enrollment.EnrollmentId,
                SectionId = enrollment.SectionId,
                StudentId = enrollment.StudentId,
                EnrolledAt = enrollment.EnrolledAt,
                Grade = enrollment.Grade,
                Status = (int)enrollment.Status,
                CourseId = enrollment.Section?.Course?.CourseId ?? 0,
                CourseName = enrollment.Section?.Course?.CourseName ?? "",
                CourseCode = enrollment.Section?.Course?.CourseCode ?? "",
                CreditHours = enrollment.Section?.Course?.CreditHours ?? 0,
                SectionName = enrollment.Section?.SectionName,
                Semester = enrollment.Section?.Semester ?? "",
                InstructorName = enrollment.Section?.Instructor?.User?.FullName ?? "",
                StudentName = enrollment.Student?.User?.FullName ?? "",
                StudentEmail = enrollment.Student?.User?.Email ?? "",
                DeclineReason = enrollment.DeclineReason,
                ApprovedBy = enrollment.ApprovedBy,
                ApprovedAt = enrollment.ApprovedAt
            };

            return Ok(ApiResponse<ViewEnrollmentDTO>.SuccessResponse(dto));
        }

        // =========================
        // Update Enrollment (Admin/Instructor - grade, status)
        // =========================
        [Authorize(Roles = "Admin,Instructor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDTO dto)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .Include(e => e.Section)
                    .ThenInclude(s => s!.Instructor)
                .Include(e => e.Section)
                    .ThenInclude(s => s!.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            // Instructors can only update grades for their own sections
            var userId = GetUserId();
            if (User.IsInRole("Instructor"))
            {
                var instructor = await unitOfWork.InstructorProfiles
                    .GetAllAsQueryable()
                    .FirstOrDefaultAsync(i => i.UserId == userId);

                if (instructor == null || enrollment.Section?.InstructorId != instructor.InstructorId)
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        ApiResponse<string>.FailureResponse("Forbidden", StatusCodes.Status403Forbidden));
            }

            // Update grade if provided
            if (!string.IsNullOrWhiteSpace(dto.Grade))
            {
                if (!GradeHelper.IsValidGrade(dto.Grade))
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid grade. Use: A, A-, B+, B, B-, C+, C, C-, D+, D, F", 400));

                enrollment.Grade = dto.Grade.ToUpper();
            }

            // Update status if provided (Admin only for status changes)
            if (dto.Status.HasValue && User.IsInRole("Admin"))
            {
                var newStatus = (Status)dto.Status.Value;
                enrollment.Status = newStatus;

                if (newStatus == Status.Declined && !string.IsNullOrWhiteSpace(dto.DeclineReason))
                    enrollment.DeclineReason = dto.DeclineReason;

                if (newStatus == Status.Enrolled || newStatus == Status.Declined)
                {
                    enrollment.ApprovedBy = userId;
                    enrollment.ApprovedAt = DateTime.UtcNow;
                }
            }

            await unitOfWork.SaveChangesAsync();

            // Keep transcript/GPA in sync with enrollment edits
            await SyncTranscriptFromEnrollmentAsync(enrollment);

            return Ok(ApiResponse<string>.SuccessResponse("Enrollment updated successfully"));
        }

        // =========================
        // Delete Enrollment (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .Include(e => e.Section)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            // Return seat to section
            if (enrollment.Section != null)
                enrollment.Section.AvailableSeats++;

            await unitOfWork.Enrollments.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse("Enrollment deleted successfully"));
        }

        // =========================
        // Drop Enrollment (Student can drop own, Admin can drop any)
        // =========================
        [HttpPost("{id}/drop")]
        public async Task<IActionResult> Drop(int id)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .Include(e => e.Section)
                    .ThenInclude(s => s!.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            var userId = GetUserId();

            // Students can only drop their own enrollments
            if (!User.IsInRole("Admin") && enrollment.Student?.UserId != userId)
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    ApiResponse<string>.FailureResponse("Forbidden", StatusCodes.Status403Forbidden));

            // Can only drop if currently enrolled or pending
            if (enrollment.Status != Status.Enrolled && enrollment.Status != Status.Pending)
                return BadRequest(ApiResponse<string>.FailureResponse("Can only drop active or pending enrollments", 400));

            // Timeline gate: allow drop during registration window OR withdraw period
            var settings = await unitOfWork.AcademicCalendarSettings.GetAllAsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SettingsKey == "default");

            var today = DateTime.UtcNow.Date;
            var regStart = settings?.RegistrationStartDateUtc?.Date;
            var regEnd = settings?.RegistrationEndDateUtc?.Date;
            var withdrawStart = settings?.WithdrawStartDateUtc?.Date;
            var withdrawEnd = settings?.WithdrawEndDateUtc?.Date;

            var inRegistrationWindow = regStart.HasValue && regEnd.HasValue && today >= regStart.Value && today <= regEnd.Value;
            var inWithdrawWindow = withdrawStart.HasValue && withdrawEnd.HasValue && today >= withdrawStart.Value && today <= withdrawEnd.Value;

            if (!inRegistrationWindow && !inWithdrawWindow)
            {
                var msg = "Dropping/withdrawing is not allowed right now. " +
                          $"Registration window: {(regStart.HasValue ? regStart.Value.ToString("yyyy-MM-dd") : "N/A")} to {(regEnd.HasValue ? regEnd.Value.ToString("yyyy-MM-dd") : "N/A")} (UTC). " +
                          $"Withdraw window: {(withdrawStart.HasValue ? withdrawStart.Value.ToString("yyyy-MM-dd") : "N/A")} to {(withdrawEnd.HasValue ? withdrawEnd.Value.ToString("yyyy-MM-dd") : "N/A")} (UTC).";
                return BadRequest(ApiResponse<string>.FailureResponse(msg, StatusCodes.Status400BadRequest));
            }

            enrollment.Status = Status.Dropped;

            // Return seat to section
            if (enrollment.Section != null)
                enrollment.Section.AvailableSeats++;

            await unitOfWork.SaveChangesAsync();

            // Ensure transcript/GPA reflects the drop/withdraw
            await SyncTranscriptFromEnrollmentAsync(enrollment);

            return Ok(ApiResponse<string>.SuccessResponse("Enrollment dropped successfully"));
        }

        // =========================
        // Approve Enrollment (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            if (enrollment.Status != Status.Pending)
                return BadRequest(ApiResponse<string>.FailureResponse("Can only approve pending enrollments", 400));

            enrollment.Status = Status.Enrolled;
            enrollment.ApprovedBy = GetUserId();
            enrollment.ApprovedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse("Enrollment approved successfully"));
        }

        // =========================
        // Decline Enrollment (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/decline")]
        public async Task<IActionResult> Decline(int id, [FromBody] EnrollmentDeclineDTO dto)
        {
            var enrollment = await unitOfWork.Enrollments
                .GetAllAsQueryable()
                .Include(e => e.Section)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
                return NotFound(ApiResponse<string>.FailureResponse("Enrollment not found", 404));

            if (enrollment.Status != Status.Pending)
                return BadRequest(ApiResponse<string>.FailureResponse("Can only decline pending enrollments", 400));

            enrollment.Status = Status.Declined;
            enrollment.DeclineReason = dto.Reason;
            enrollment.ApprovedBy = GetUserId();
            enrollment.ApprovedAt = DateTime.UtcNow;

            // Return seat to section
            if (enrollment.Section != null)
                enrollment.Section.AvailableSeats++;

            await unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse("Enrollment declined"));
        }
    }

    // Additional DTOs
    public class UpdateEnrollmentDTO
    {
        public string? Grade { get; set; }
        public int? Status { get; set; }
        public string? DeclineReason { get; set; }
    }

    public class EnrollmentDeclineDTO
    {
        public string? Reason { get; set; }
    }
}
