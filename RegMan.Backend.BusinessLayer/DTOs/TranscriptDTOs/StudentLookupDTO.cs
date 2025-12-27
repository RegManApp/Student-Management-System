namespace RegMan.Backend.BusinessLayer.DTOs.TranscriptDTOs
{
    public class StudentLookupDTO
    {
        public string StudentUserId { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
