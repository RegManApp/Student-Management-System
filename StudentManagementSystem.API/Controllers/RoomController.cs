using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.API.Common;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.RoomDTOs;

namespace StudentManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;

        public RoomController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        // =========================
        // GET (Public / Authenticated)
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await roomService.GetAllRoomsAsync();
            return Ok(ApiResponse<IEnumerable<ViewRoomDTO>>.SuccessResponse(rooms));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await roomService.GetRoomByIdAsync(id);
            return Ok(ApiResponse<ViewRoomDTO>.SuccessResponse(room));
        }

        // =========================
        // CREATE (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDTO dto)
        {
            var room = await roomService.CreateRoomAsync(dto);
            return Ok(ApiResponse<ViewRoomDTO>.SuccessResponse(room));
        }

        // =========================
        // UPDATE (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoomDTO dto)
        {
            var room = await roomService.UpdateRoomAsync(dto);
            return Ok(ApiResponse<ViewRoomDTO>.SuccessResponse(room));
        }

        // =========================
        // DELETE (Admin only)
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await roomService.DeleteRoomAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponse<string>.FailureResponse(
                    "Room not found",
                    StatusCodes.Status404NotFound
                ));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Room deleted successfully"));
        }
    }
}
