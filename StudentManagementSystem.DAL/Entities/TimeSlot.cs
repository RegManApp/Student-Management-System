using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.DAL.Entities
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotId { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        // Simple validation helper
        public bool IsValid()
        {
            return EndTime > StartTime;
        }
    }
}
