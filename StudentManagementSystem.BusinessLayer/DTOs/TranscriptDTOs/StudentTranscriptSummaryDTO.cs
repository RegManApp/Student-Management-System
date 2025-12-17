namespace StudentManagementSystem.BusinessLayer.DTOs.TranscriptDTOs
{
    public class StudentTranscriptSummaryDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public double CumulativeGPA { get; set; }
        public int TotalCreditsCompleted { get; set; }
        public int TotalCreditsRequired { get; set; }
        public double CompletionPercentage { get; set; }
        public IEnumerable<SemesterTranscriptDTO> Semesters { get; set; } = new List<SemesterTranscriptDTO>();
    }

    public class SemesterTranscriptDTO
    {
        public string Semester { get; set; } = string.Empty;
        public int Year { get; set; }
        public double SemesterGPA { get; set; }
        public int SemesterCredits { get; set; }
        public IEnumerable<ViewTranscriptDTO> Courses { get; set; } = new List<ViewTranscriptDTO>();
    }
}
