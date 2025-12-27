namespace RegMan.Backend.BusinessLayer.DTOs.TranscriptDTOs
{
    public class StudentTranscriptSummaryDTO
    {
        public TranscriptHeaderDTO Header { get; set; } = new();

        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public double CumulativeGPA { get; set; }
        public int TotalCreditsCompleted { get; set; }
        public int TotalCreditsRequired { get; set; }
        public double CompletionPercentage { get; set; }
        public IEnumerable<SemesterTranscriptDTO> Semesters { get; set; } = new List<SemesterTranscriptDTO>();

        public TranscriptOverallSummaryDTO OverallSummary { get; set; } = new();
        public IEnumerable<TranscriptTestScoreDTO> TestScores { get; set; } = new List<TranscriptTestScoreDTO>();
    }

    public class TranscriptHeaderDTO
    {
        public string TranscriptType { get; set; } = string.Empty; // e.g., Unofficial
        public string UniversityName { get; set; } = string.Empty;
        public string RegistrarOfficeName { get; set; } = string.Empty;
        public IEnumerable<string> RegistrarAddressLines { get; set; } = new List<string>();

        public string StudentFullName { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentEmail { get; set; } = string.Empty;

        public string? ProgramOrDegree { get; set; }
        public string? Curriculum { get; set; }
        public bool? DegreeAwarded { get; set; }
        public string? Honors { get; set; }
        public string? PreviousInstitution { get; set; }
    }

    public class TranscriptOverallSummaryDTO
    {
        public int AttemptedCredits { get; set; }
        public int EarnedCredits { get; set; }
        public int GpaCredits { get; set; }
        public int TransferCredits { get; set; }
        public double QualityPoints { get; set; }
        public double GPA { get; set; }
    }

    public class TranscriptTermSummaryDTO
    {
        public int AttemptedCredits { get; set; }
        public int EarnedCredits { get; set; }
        public int GpaCredits { get; set; }
        public int TransferCredits { get; set; }
        public double QualityPoints { get; set; }
        public double GPA { get; set; }
    }

    public class TranscriptTestScoreDTO
    {
        public string TestName { get; set; } = string.Empty;
        public string? Score { get; set; }
        public DateTime? TakenAt { get; set; }
    }

    public class SemesterTranscriptDTO
    {
        public string Semester { get; set; } = string.Empty;
        public int Year { get; set; }

        // Human-friendly term label (e.g., "2023 Fall")
        public string TermName { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;

        public double SemesterGPA { get; set; }
        public int SemesterCredits { get; set; }

        public TranscriptTermSummaryDTO Summary { get; set; } = new();
        public IEnumerable<ViewTranscriptDTO> Courses { get; set; } = new List<ViewTranscriptDTO>();
    }
}
