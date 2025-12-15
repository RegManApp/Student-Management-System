using StudentManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SlotType SlotType { get; set; } = SlotType.Lecture; //lecture by default
    }
}
