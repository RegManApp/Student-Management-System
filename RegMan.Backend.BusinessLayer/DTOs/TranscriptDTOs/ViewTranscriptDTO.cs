namespace RegMan.Backend.BusinessLayer.DTOs.TranscriptDTOs
{
    public class ViewTranscriptDTO
    {
        public int TranscriptId { get; set; }
        public int? EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string? SubType { get; set; }
        // Status codes: COMPLETED, IN_PROGRESS, WITHDRAWN
        public string Status { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public string Grade { get; set; } = string.Empty;
        public double GradePoints { get; set; }
        public double? QualityPoints { get; set; }
        public bool CountsTowardGpa { get; set; }
        public string Semester { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
