using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.TimeSlotDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            this.timeSlotService = timeSlotService;
        }

        // =========================
        // GET (Public / Authenticated)
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slots = await timeSlotService.GetAllTimeSlotsAsync();
            return Ok(ApiResponse<IEnumerable<ViewTimeSlotDTO>>.SuccessResponse(slots));
        }

        // =========================
        // CREATE (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimeSlotDTO dto)
        {
            var slot = await timeSlotService.CreateTimeSlotAsync(dto);
            return Ok(ApiResponse<ViewTimeSlotDTO>.SuccessResponse(slot));
        }

        // =========================
        // DELETE (Admin only)
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
