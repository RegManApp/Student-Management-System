namespace RegMan.Backend.BusinessLayer.DTOs.TimeSlotDTOs
{
    public class ViewTimeSlotDTO
    {
        public int TimeSlotId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
