using System.ComponentModel.DataAnnotations;

namespace RegMan.Backend.BusinessLayer.DTOs.TimeSlotDTOs
{
    public class CreateTimeSlotDTO
    {
        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}
