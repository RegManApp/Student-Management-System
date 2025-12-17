using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.RoomDTOs;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace StudentManagementSystem.BusinessLayer.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuditLogService auditLogService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RoomService(
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

            return (
                user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? throw new Exception("UserId not found."),
                user.FindFirstValue(ClaimTypes.Email)
                    ?? "unknown@email.com"
            );
        }

        // =========================
        // Create
        // =========================
        public async Task<ViewRoomDTO> CreateRoomAsync(CreateRoomDTO dto)
        {
            var room = new Room
            {
                Building = dto.Building,
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                Type = dto.Type
            };

            await unitOfWork.Rooms.AddAsync(room);
            await unitOfWork.SaveChangesAsync();

            // Audit Log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(
                userId,
                email,
                "CREATE",
                "Room",
                room.RoomId.ToString()
            );

            return MapToView(room);
        }

        // =========================
        // Update
        // =========================
        public async Task<ViewRoomDTO> UpdateRoomAsync(UpdateRoomDTO dto)
        {
            var room = await unitOfWork.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null)
                throw new Exception($"Room with ID {dto.RoomId} not found.");

            room.Building = dto.Building;
            room.RoomNumber = dto.RoomNumber;
            room.Capacity = dto.Capacity;
            room.Type = dto.Type;

            unitOfWork.Rooms.Update(room);
            await unitOfWork.SaveChangesAsync();

            // Audit Log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(
                userId,
                email,
                "UPDATE",
                "Room",
                room.RoomId.ToString()
            );

            return MapToView(room);
        }

        // =========================
        // Delete
        // =========================
        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            bool deleted = await unitOfWork.Rooms.DeleteAsync(roomId);
            if (!deleted)
                return false;

            await unitOfWork.SaveChangesAsync();

            // Audit Log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(
                userId,
                email,
                "DELETE",
                "Room",
                roomId.ToString()
            );

            return true;
        }

        // =========================
        // Get
        // =========================
        public async Task<ViewRoomDTO> GetRoomByIdAsync(int roomId)
        {
            var room = await unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
                throw new Exception($"Room with ID {roomId} not found.");

            return MapToView(room);
        }

        public async Task<IEnumerable<ViewRoomDTO>> GetAllRoomsAsync()
        {
            var rooms = await unitOfWork.Rooms.GetAllAsQueryable().ToListAsync();
            return rooms.Select(MapToView);
        }

        // =========================
        // Mapping
        // =========================
        private static ViewRoomDTO MapToView(Room room)
        {
            return new ViewRoomDTO
            {
                RoomId = room.RoomId,
                Building = room.Building,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                Type = room.Type
            };
        }
    }
}
