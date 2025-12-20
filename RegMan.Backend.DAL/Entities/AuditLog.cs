using System.ComponentModel.DataAnnotations;

namespace RegMan.Backend.DAL.Entities
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string UserEmail { get; set; } = null!;

        [Required]
        public string Action { get; set; } = null!; // CREATE / UPDATE / DELETE

        [Required]
        public string EntityName { get; set; } = null!; // Course, Student, ...

        [Required]
        public string EntityId { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
