using System.ComponentModel.DataAnnotations;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.DTOs.ScheduleSlotDTOs
{
    public class CreateScheduleSlotDTO
    {
        [Required]
        public int SectionId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int TimeSlotId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public SlotType SlotType { get; set; }
    }
}
