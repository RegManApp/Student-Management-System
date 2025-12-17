namespace StudentManagementSystem.BusinessLayer.DTOs.ScheduleSlotDTOs
{
    public class ViewScheduleSlotDTO
    {
        public int ScheduleSlotId { get; set; }

        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;

        public int RoomId { get; set; }
        public string Room { get; set; } = string.Empty;

        public int TimeSlotId { get; set; }
        public string TimeSlot { get; set; } = string.Empty;

        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;

        public string SlotType { get; set; } = string.Empty;
    }
}
