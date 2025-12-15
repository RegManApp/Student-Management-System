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

        // =========================
        // Create Schedule Slot
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleSlotDTO dto)
        {
            var result = await scheduleSlotService.CreateAsync(dto);
            return Ok(result);
        }

        // =========================
        // Get All Schedule Slots
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await scheduleSlotService.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // Get By Section
        // =========================
        [HttpGet("section/{sectionId:int}")]
        public async Task<IActionResult> GetBySection(int sectionId)
        {
            var result = await scheduleSlotService.GetBySectionAsync(sectionId);
            return Ok(result);
        }

        // =========================
        // Get By Instructor
        // =========================
        [HttpGet("instructor/{instructorId:int}")]
        public async Task<IActionResult> GetByInstructor(int instructorId)
        {
            var result = await scheduleSlotService.GetByInstructorAsync(instructorId);
            return Ok(result);
        }

        // =========================
        // Get By Room
        // =========================
        [HttpGet("room/{roomId:int}")]
        public async Task<IActionResult> GetByRoom(int roomId)
        {
            var result = await scheduleSlotService.GetByRoomAsync(roomId);
            return Ok(result);
        }
    }
}
