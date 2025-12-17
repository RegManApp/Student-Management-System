using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using System.Security.Claims;

namespace StudentManagementSystem.BusinessLayer.Services
{
    internal class TranscriptService : ITranscriptService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuditLogService auditLogService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TranscriptService(
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.auditLogService = auditLogService;
            this.httpContextAccessor = httpContextAccessor;
        }

        // =========================
        // Helpers
        // =========================
        private (string userId, string email) GetUserInfo()
        {
            var user = httpContextAccessor.HttpContext?.User
                ?? throw new Exception("User context not found.");

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("UserId not found.");

            var email = user.FindFirstValue(ClaimTypes.Email)
                ?? "unknown@email.com";

            return (userId, email);
        }

        // =====================================
        // Create Transcript (Admin/Instructor)
        // =====================================
        public async Task<ViewTranscriptDTO> CreateTranscriptAsync(CreateTranscriptDTO dto)
        {
            // Validate grade
            if (!GradeHelper.IsValidGrade(dto.Grade))
                throw new ArgumentException($"Invalid grade: {dto.Grade}");

            // Check student exists
            var student = await unitOfWork.StudentProfiles
                .GetAllAsQueryable()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId)
                ?? throw new Exception($"Student with ID {dto.StudentId} not found.");

            // Check course exists
            var course = await unitOfWork.Courses.GetByIdAsync(dto.CourseId)
                ?? throw new Exception($"Course with ID {dto.CourseId} not found.");

            // Check section exists
            var section = await unitOfWork.Sections.GetByIdAsync(dto.SectionId)
                ?? throw new Exception($"Section with ID {dto.SectionId} not found.");

            // Check if transcript already exists for this student-course-section
            var existingTranscript = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .AnyAsync(t => t.StudentId == dto.StudentId && t.CourseId == dto.CourseId && t.SectionId == dto.SectionId);

            if (existingTranscript)
                throw new Exception("Transcript entry already exists for this student, course, and section.");

            var transcript = new Transcript
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                SectionId = dto.SectionId,
                Grade = dto.Grade.ToUpper(),
                GradePoints = GradeHelper.GetGradePoints(dto.Grade),
                Semester = dto.Semester,
                Year = dto.Year,
                CreditHours = course.CreditHours,
                CompletedAt = DateTime.UtcNow
            };

            await unitOfWork.Transcripts.AddAsync(transcript);
            await unitOfWork.SaveChangesAsync();

            // Update student's completed credits and GPA
            await RecalculateAndUpdateStudentGPAAsync(dto.StudentId);

            // Audit log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(userId, email, "CREATE", "Transcript", transcript.TranscriptId.ToString());

            return new ViewTranscriptDTO
            {
                TranscriptId = transcript.TranscriptId,
                StudentId = transcript.StudentId,
                StudentName = student.User.FullName,
                CourseId = transcript.CourseId,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                CreditHours = transcript.CreditHours,
                Grade = transcript.Grade,
                GradePoints = transcript.GradePoints,
                Semester = transcript.Semester,
                Year = transcript.Year,
                CompletedAt = transcript.CompletedAt
            };
        }

        // =====================================
        // Update Grade (Admin/Instructor)
        // =====================================
        public async Task<ViewTranscriptDTO> UpdateGradeAsync(UpdateTranscriptDTO dto)
        {
            if (!GradeHelper.IsValidGrade(dto.Grade))
                throw new ArgumentException($"Invalid grade: {dto.Grade}");

            var transcript = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Include(t => t.Student)
                    .ThenInclude(s => s.User)
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.TranscriptId == dto.TranscriptId)
                ?? throw new Exception($"Transcript with ID {dto.TranscriptId} not found.");

            transcript.Grade = dto.Grade.ToUpper();
            transcript.GradePoints = GradeHelper.GetGradePoints(dto.Grade);

            unitOfWork.Transcripts.Update(transcript);
            await unitOfWork.SaveChangesAsync();

            // Recalculate GPA
            await RecalculateAndUpdateStudentGPAAsync(transcript.StudentId);

            // Audit log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(userId, email, "UPDATE", "Transcript", transcript.TranscriptId.ToString());

            return new ViewTranscriptDTO
            {
                TranscriptId = transcript.TranscriptId,
                StudentId = transcript.StudentId,
                StudentName = transcript.Student.User.FullName,
                CourseId = transcript.CourseId,
                CourseName = transcript.Course.CourseName,
                CourseCode = transcript.Course.CourseCode,
                CreditHours = transcript.CreditHours,
                Grade = transcript.Grade,
                GradePoints = transcript.GradePoints,
                Semester = transcript.Semester,
                Year = transcript.Year,
                CompletedAt = transcript.CompletedAt
            };
        }

        // =====================================
        // Delete Transcript (Admin)
        // =====================================
        public async Task<string> DeleteTranscriptAsync(int transcriptId)
        {
            var transcript = await unitOfWork.Transcripts.GetByIdAsync(transcriptId)
                ?? throw new Exception($"Transcript with ID {transcriptId} not found.");

            var studentId = transcript.StudentId;

            await unitOfWork.Transcripts.DeleteAsync(transcriptId);
            await unitOfWork.SaveChangesAsync();

            // Recalculate GPA after deletion
            await RecalculateAndUpdateStudentGPAAsync(studentId);

            // Audit log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(userId, email, "DELETE", "Transcript", transcriptId.ToString());

            return $"Transcript with ID {transcriptId} deleted successfully.";
        }

        // =====================================
        // Get Transcript By ID
        // =====================================
        public async Task<ViewTranscriptDTO> GetTranscriptByIdAsync(int transcriptId)
        {
            var transcript = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Include(t => t.Student)
                    .ThenInclude(s => s.User)
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.TranscriptId == transcriptId)
                ?? throw new Exception($"Transcript with ID {transcriptId} not found.");

            return new ViewTranscriptDTO
            {
                TranscriptId = transcript.TranscriptId,
                StudentId = transcript.StudentId,
                StudentName = transcript.Student.User.FullName,
                CourseId = transcript.CourseId,
                CourseName = transcript.Course.CourseName,
                CourseCode = transcript.Course.CourseCode,
                CreditHours = transcript.CreditHours,
                Grade = transcript.Grade,
                GradePoints = transcript.GradePoints,
                Semester = transcript.Semester,
                Year = transcript.Year,
                CompletedAt = transcript.CompletedAt
            };
        }

        // =====================================
        // Get Transcripts By Student ID
        // =====================================
        public async Task<IEnumerable<ViewTranscriptDTO>> GetTranscriptsByStudentIdAsync(int studentId)
        {
            return await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Include(t => t.Student)
                    .ThenInclude(s => s.User)
                .Include(t => t.Course)
                .Where(t => t.StudentId == studentId)
                .OrderByDescending(t => t.Year)
                .ThenBy(t => t.Semester)
                .Select(t => new ViewTranscriptDTO
                {
                    TranscriptId = t.TranscriptId,
                    StudentId = t.StudentId,
                    StudentName = t.Student.User.FullName,
                    CourseId = t.CourseId,
                    CourseName = t.Course.CourseName,
                    CourseCode = t.Course.CourseCode,
                    CreditHours = t.CreditHours,
                    Grade = t.Grade,
                    GradePoints = t.GradePoints,
                    Semester = t.Semester,
                    Year = t.Year,
                    CompletedAt = t.CompletedAt
                })
                .ToListAsync();
        }

        // =====================================
        // Get Transcripts By Semester
        // =====================================
        public async Task<IEnumerable<ViewTranscriptDTO>> GetTranscriptsBySemesterAsync(string semester, int year)
        {
            return await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Include(t => t.Student)
                    .ThenInclude(s => s.User)
                .Include(t => t.Course)
                .Where(t => t.Semester == semester && t.Year == year)
                .Select(t => new ViewTranscriptDTO
                {
                    TranscriptId = t.TranscriptId,
                    StudentId = t.StudentId,
                    StudentName = t.Student.User.FullName,
                    CourseId = t.CourseId,
                    CourseName = t.Course.CourseName,
                    CourseCode = t.Course.CourseCode,
                    CreditHours = t.CreditHours,
                    Grade = t.Grade,
                    GradePoints = t.GradePoints,
                    Semester = t.Semester,
                    Year = t.Year,
                    CompletedAt = t.CompletedAt
                })
                .ToListAsync();
        }

        // =====================================
        // Get Student Full Transcript
        // =====================================
        public async Task<StudentTranscriptSummaryDTO> GetStudentFullTranscriptAsync(string studentUserId)
        {
            var student = await unitOfWork.StudentProfiles
                .GetAllAsQueryable()
                .Include(s => s.User)
                .Include(s => s.AcademicPlan)
                .Include(s => s.Transcripts)
                    .ThenInclude(t => t.Course)
                .FirstOrDefaultAsync(s => s.UserId == studentUserId)
                ?? throw new Exception("Student not found.");

            return BuildStudentTranscriptSummary(student);
        }

        // =====================================
        // Get My Transcript (Student)
        // =====================================
        public async Task<StudentTranscriptSummaryDTO> GetMyTranscriptAsync(string userId)
        {
            return await GetStudentFullTranscriptAsync(userId);
        }

        // =====================================
        // Calculate Student GPA
        // =====================================
        public async Task<double> CalculateStudentGPAAsync(int studentId)
        {
            var transcripts = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Where(t => t.StudentId == studentId)
                .ToListAsync();

            if (!transcripts.Any())
                return 0.0;

            double totalQualityPoints = transcripts.Sum(t => t.GradePoints * t.CreditHours);
            int totalCredits = transcripts.Sum(t => t.CreditHours);

            return totalCredits > 0 ? Math.Round(totalQualityPoints / totalCredits, 2) : 0.0;
        }

        // =====================================
        // Calculate Semester GPA
        // =====================================
        public async Task<double> CalculateSemesterGPAAsync(int studentId, string semester, int year)
        {
            var transcripts = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Where(t => t.StudentId == studentId && t.Semester == semester && t.Year == year)
                .ToListAsync();

            if (!transcripts.Any())
                return 0.0;

            double totalQualityPoints = transcripts.Sum(t => t.GradePoints * t.CreditHours);
            int totalCredits = transcripts.Sum(t => t.CreditHours);

            return totalCredits > 0 ? Math.Round(totalQualityPoints / totalCredits, 2) : 0.0;
        }

        // =====================================
        // Recalculate And Update Student GPA
        // =====================================
        public async Task RecalculateAndUpdateStudentGPAAsync(int studentId)
        {
            var student = await unitOfWork.StudentProfiles.GetByIdAsync(studentId)
                ?? throw new Exception($"Student with ID {studentId} not found.");

            var transcripts = await unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Where(t => t.StudentId == studentId)
                .ToListAsync();

            if (!transcripts.Any())
            {
                student.GPA = 0.0;
                student.CompletedCredits = 0;
            }
            else
            {
                double totalQualityPoints = transcripts.Sum(t => t.GradePoints * t.CreditHours);
                int totalCredits = transcripts.Sum(t => t.CreditHours);

                student.GPA = totalCredits > 0 ? Math.Round(totalQualityPoints / totalCredits, 2) : 0.0;
                student.CompletedCredits = transcripts
                    .Where(t => GradeHelper.IsPassing(t.Grade))
                    .Sum(t => t.CreditHours);
            }

            unitOfWork.StudentProfiles.Update(student);
            await unitOfWork.SaveChangesAsync();
        }

        // =====================================
        // Get All Transcripts (Admin)
        // =====================================
        public async Task<IEnumerable<ViewTranscriptDTO>> GetAllTranscriptsAsync(
            int? studentId = null,
            int? courseId = null,
            string? semester = null,
            int? year = null,
            string? grade = null)
        {
            var query = unitOfWork.Transcripts
                .GetAllAsQueryable()
                .Include(t => t.Student)
                    .ThenInclude(s => s.User)
                .Include(t => t.Course)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(t => t.StudentId == studentId.Value);

            if (courseId.HasValue)
                query = query.Where(t => t.CourseId == courseId.Value);

            if (!string.IsNullOrWhiteSpace(semester))
                query = query.Where(t => t.Semester == semester);

            if (year.HasValue)
                query = query.Where(t => t.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(grade))
                query = query.Where(t => t.Grade == grade.ToUpper());

            return await query
                .OrderByDescending(t => t.Year)
                .ThenBy(t => t.Semester)
                .Select(t => new ViewTranscriptDTO
                {
                    TranscriptId = t.TranscriptId,
                    StudentId = t.StudentId,
                    StudentName = t.Student.User.FullName,
                    CourseId = t.CourseId,
                    CourseName = t.Course.CourseName,
                    CourseCode = t.Course.CourseCode,
                    CreditHours = t.CreditHours,
                    Grade = t.Grade,
                    GradePoints = t.GradePoints,
                    Semester = t.Semester,
                    Year = t.Year,
                    CompletedAt = t.CompletedAt
                })
                .ToListAsync();
        }

        // =========================
        // Helper: Build Summary DTO
        // =========================
        private StudentTranscriptSummaryDTO BuildStudentTranscriptSummary(StudentProfile student)
        {
            var transcripts = student.Transcripts.ToList();

            // Group by semester and year
            var semesterGroups = transcripts
                .GroupBy(t => new { t.Semester, t.Year })
                .OrderByDescending(g => g.Key.Year)
                .ThenByDescending(g => g.Key.Semester)
                .Select(g => new SemesterTranscriptDTO
                {
                    Semester = g.Key.Semester,
                    Year = g.Key.Year,
                    SemesterGPA = g.Sum(t => t.CreditHours) > 0
                        ? Math.Round(g.Sum(t => t.GradePoints * t.CreditHours) / g.Sum(t => t.CreditHours), 2)
                        : 0.0,
                    SemesterCredits = g.Sum(t => t.CreditHours),
                    Courses = g.Select(t => new ViewTranscriptDTO
                    {
                        TranscriptId = t.TranscriptId,
                        StudentId = t.StudentId,
                        StudentName = student.User.FullName,
                        CourseId = t.CourseId,
                        CourseName = t.Course.CourseName,
                        CourseCode = t.Course.CourseCode,
                        CreditHours = t.CreditHours,
                        Grade = t.Grade,
                        GradePoints = t.GradePoints,
                        Semester = t.Semester,
                        Year = t.Year,
                        CompletedAt = t.CompletedAt
                    }).ToList()
                })
                .ToList();

            int totalCreditsCompleted = transcripts.Where(t => GradeHelper.IsPassing(t.Grade)).Sum(t => t.CreditHours);
            int totalCreditsRequired = student.AcademicPlan?.TotalCreditsRequired ?? 0;

            return new StudentTranscriptSummaryDTO
            {
                StudentId = student.StudentId,
                StudentName = student.User.FullName,
                Email = student.User.Email ?? string.Empty,
                MajorName = student.AcademicPlan?.MajorName ?? "Not Assigned",
                CumulativeGPA = student.GPA,
                TotalCreditsCompleted = totalCreditsCompleted,
                TotalCreditsRequired = totalCreditsRequired,
                CompletionPercentage = totalCreditsRequired > 0
                    ? Math.Round((double)totalCreditsCompleted / totalCreditsRequired * 100, 2)
                    : 0.0,
                Semesters = semesterGroups
            };
        }
    }
}
