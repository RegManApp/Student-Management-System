using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BusinessLayer.Contracts;
using StudentManagementSystem.BusinessLayer.DTOs.TimeSlotDTOs;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace StudentManagementSystem.BusinessLayer.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuditLogService auditLogService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TimeSlotService(
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.auditLogService = auditLogService;
            this.httpContextAccessor = httpContextAccessor;
        }

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
        public async Task<ViewTimeSlotDTO> CreateTimeSlotAsync(CreateTimeSlotDTO dto)
        {
            var slot = new TimeSlot
            {
                Day = dto.Day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            if (!slot.IsValid())
                throw new Exception("End time must be greater than start time.");

            await unitOfWork.TimeSlots.AddAsync(slot);
            await unitOfWork.SaveChangesAsync();

            // Audit Log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(
                userId,
                email,
                "CREATE",
                "TimeSlot",
                slot.TimeSlotId.ToString()
            );

            return new ViewTimeSlotDTO
            {
                TimeSlotId = slot.TimeSlotId,
                Day = slot.Day,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime
            };
        }

        // =========================
        // Delete
        // =========================
        public async Task<bool> DeleteTimeSlotAsync(int timeSlotId)
        {
            bool deleted = await unitOfWork.TimeSlots.DeleteAsync(timeSlotId);
            if (!deleted)
                return false;

            await unitOfWork.SaveChangesAsync();

            // Audit Log
            var (userId, email) = GetUserInfo();
            await auditLogService.LogAsync(
                userId,
                email,
                "DELETE",
                "TimeSlot",
                timeSlotId.ToString()
            );

            return true;
        }

        // =========================
        // Get
        // =========================
        public async Task<IEnumerable<ViewTimeSlotDTO>> GetAllTimeSlotsAsync()
        {
            var slots = await unitOfWork.TimeSlots.GetAllAsQueryable().ToListAsync();

            return slots.Select(s => new ViewTimeSlotDTO
            {
                TimeSlotId = s.TimeSlotId,
                Day = s.Day,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            });
        }
    }
}
