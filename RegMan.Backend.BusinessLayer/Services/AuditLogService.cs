using RegMan.Backend.BusinessLayer.Contracts;
using RegMan.Backend.DAL.DataContext;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext context;

        public AuditLogService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task LogAsync(
            string userId,
            string userEmail,
            string action,
            string entityName,
            string entityId)
        {
            var log = new AuditLog
            {
                UserId = userId,
                UserEmail = userEmail,
                Action = action,
                EntityName = entityName,
                EntityId = entityId
            };

            context.AuditLogs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}
