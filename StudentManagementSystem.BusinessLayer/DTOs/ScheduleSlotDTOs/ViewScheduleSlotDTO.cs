using StudentManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs
{
    public class ViewScheduleSlotDTO
    {
        public int ScheduleSlotId { get; set; }
        public SlotType SlotType { get; set; }
        public int SectionId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public int TimeSlotId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; } //OR summarize all in a timeslot DTO
    }
}
