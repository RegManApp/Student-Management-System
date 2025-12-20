using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegMan.Backend.API.Common;
using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.BusinessLayer.DTOs.TranscriptDTOs;
using System.Security.Claims;

namespace RegMan.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GpaController : ControllerBase
    {
        private readonly ITranscriptService transcriptService;

        public GpaController(ITranscriptService transcriptService)
        {
            this.transcriptService = transcriptService;
        }

        // POST: /api/gpa/simulate
        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateGPAAsync([FromBody] SimulateGpaRequestDTO dto)
        {
            int? studentId = dto.StudentId;

            if (!studentId.HasValue)
            {
                // try to resolve current student's id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return BadRequest(ApiResponse<string>.FailureResponse("User not found.", 400));

                var summary = await transcriptService.GetMyTranscriptAsync(userId);
                studentId = summary.StudentId;
            }

            if (!studentId.HasValue || studentId.Value <= 0)
                return BadRequest(ApiResponse<string>.FailureResponse("StudentId is required.", 400));

            // current GPA (use existing calculation)
            var currentGpa = await transcriptService.CalculateStudentGPAAsync(studentId.Value);

            // simulated GPA
            var simulatedGpa = await transcriptService.CalculateSimulatedGPAAsync(studentId.Value, dto.SimulatedCourses);

            var response = new SimulateGpaResponseDTO
            {
                CurrentGPA = currentGpa,
                SimulatedGPA = simulatedGpa
            };

            return Ok(ApiResponse<SimulateGpaResponseDTO>.SuccessResponse(response, "Simulated GPA calculated"));
        }
    }
}
