using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleSlotController : ControllerBase
    {
        private readonly IScheduleSlotService scheduleSlotService;
        public ScheduleSlotController(IScheduleSlotService scheduleSlotService)
        {
            this.scheduleSlotService = scheduleSlotService;
        }
        [HttpPost]
        public async Task<IActionResult> AddScheduleSlot([FromBody] CreateScheduleSlotDTO scheduleSlot)
        {
            try 
            {
                var result = await scheduleSlotService.AddScheduleSlotAsync(scheduleSlot);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            
        }
    }
}
