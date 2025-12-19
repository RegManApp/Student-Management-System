using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DAL.Contracts;
using StudentManagementSystem.DAL.DataContext;
using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.BusinessLayer.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, NotificationType type, string title, string message, string? entityType = null, int? entityId = null);
        Task CreateOfficeHourBookedNotificationAsync(int bookingId, string instructorUserId, string studentName, DateTime date, TimeSpan startTime);
        Task CreateOfficeHourCancelledNotificationAsync(string userId, string cancelledBy, DateTime date, TimeSpan startTime, string? reason = null);
        Task CreateOfficeHourConfirmedNotificationAsync(string studentUserId, string instructorName, DateTime date, TimeSpan startTime);
        Task CreateEnrollmentApprovedNotificationAsync(string studentUserId, string courseName, string sectionName);
        Task CreateEnrollmentDeclinedNotificationAsync(string studentUserId, string courseName, string sectionName, string? reason = null);
        Task CreateEnrollmentPendingNotificationAsync(string instructorUserId, string studentName, string courseName, string sectionName, int enrollmentId);
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(string userId, NotificationType type, string title, string message, string? entityType = null, int? entityId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                EntityType = entityType,
                EntityId = entityId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateOfficeHourBookedNotificationAsync(int bookingId, string instructorUserId, string studentName, DateTime date, TimeSpan startTime)
        {
            var formattedDate = date.ToString("dddd, MMMM d, yyyy");
            var formattedTime = DateTime.Today.Add(startTime).ToString("h:mm tt");

            await CreateNotificationAsync(
                instructorUserId,
                NotificationType.OfficeHourBooked,
                "New Office Hour Booking",
                $"{studentName} has booked an office hour with you on {formattedDate} at {formattedTime}.",
                "OfficeHourBooking",
                bookingId
            );
        }

        public async Task CreateOfficeHourCancelledNotificationAsync(string userId, string cancelledBy, DateTime date, TimeSpan startTime, string? reason = null)
        {
            var formattedDate = date.ToString("dddd, MMMM d, yyyy");
            var formattedTime = DateTime.Today.Add(startTime).ToString("h:mm tt");
            var message = $"Office hour on {formattedDate} at {formattedTime} has been cancelled by {cancelledBy}.";
            if (!string.IsNullOrEmpty(reason))
            {
                message += $" Reason: {reason}";
            }

            await CreateNotificationAsync(
                userId,
                NotificationType.OfficeHourCancelled,
                "Office Hour Cancelled",
                message
            );
        }

        public async Task CreateOfficeHourConfirmedNotificationAsync(string studentUserId, string instructorName, DateTime date, TimeSpan startTime)
        {
            var formattedDate = date.ToString("dddd, MMMM d, yyyy");
            var formattedTime = DateTime.Today.Add(startTime).ToString("h:mm tt");

            await CreateNotificationAsync(
                studentUserId,
                NotificationType.OfficeHourConfirmed,
                "Office Hour Confirmed",
                $"Your office hour with {instructorName} on {formattedDate} at {formattedTime} has been confirmed.",
                null,
                null
            );
        }

        public async Task CreateEnrollmentApprovedNotificationAsync(string studentUserId, string courseName, string sectionName)
        {
            await CreateNotificationAsync(
                studentUserId,
                NotificationType.EnrollmentApproved,
                "Enrollment Approved",
                $"Your enrollment in {courseName} (Section {sectionName}) has been approved.",
                "Enrollment",
                null
            );
        }

        public async Task CreateEnrollmentDeclinedNotificationAsync(string studentUserId, string courseName, string sectionName, string? reason = null)
        {
            var message = $"Your enrollment in {courseName} (Section {sectionName}) has been declined.";
            if (!string.IsNullOrEmpty(reason))
            {
                message += $" Reason: {reason}";
            }

            await CreateNotificationAsync(
                studentUserId,
                NotificationType.EnrollmentDeclined,
                "Enrollment Declined",
                message,
                "Enrollment",
                null
            );
        }

        public async Task CreateEnrollmentPendingNotificationAsync(string instructorUserId, string studentName, string courseName, string sectionName, int enrollmentId)
        {
            await CreateNotificationAsync(
                instructorUserId,
                NotificationType.General,
                "New Enrollment Request",
                $"{studentName} has requested enrollment in {courseName} (Section {sectionName}) and is pending your approval.",
                "Enrollment",
                enrollmentId
            );
        }
    }
}
