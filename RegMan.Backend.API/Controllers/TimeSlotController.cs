using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegMan.Backend.API.Common;
using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.BusinessLayer.DTOs.TimeSlotDTOs;

namespace RegMan.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // أي request لازم يكون عامل Login
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            this.timeSlotService = timeSlotService;
        }

        // =========================
        // GET ALL
        // Admin + Instructor + Student
        // =========================
        [Authorize(Roles = "Admin,Instructor,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slots = await timeSlotService.GetAllTimeSlotsAsync();
            return Ok(ApiResponse<IEnumerable<ViewTimeSlotDTO>>.SuccessResponse(slots));
        }

        // =========================
        // CREATE
        // Admin only
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimeSlotDTO dto)
        {
            var slot = await timeSlotService.CreateTimeSlotAsync(dto);
            return Ok(ApiResponse<ViewTimeSlotDTO>.SuccessResponse(slot));
        }

        // =========================
        // DELETE
        // Admin only
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await timeSlotService.DeleteTimeSlotAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponse<string>.FailureResponse(
                    "TimeSlot not found",
                    StatusCodes.Status404NotFound
                ));
            }

            return Ok(ApiResponse<string>.SuccessResponse("TimeSlot deleted successfully"));
        }
    }
}
