using System.ComponentModel.DataAnnotations;
using StudentManagementSystem.DAL.Entities;

namespace StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs
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
