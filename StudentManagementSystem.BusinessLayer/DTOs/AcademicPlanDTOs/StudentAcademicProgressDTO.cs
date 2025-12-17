namespace StudentManagementSystem.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class StudentAcademicProgressDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanId { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public int TotalCreditsRequired { get; set; }
        public int CreditsCompleted { get; set; }
        public int CreditsRemaining { get; set; }
        public double ProgressPercentage { get; set; }
        public double CurrentGPA { get; set; }
        public int ExpectedGraduationYear { get; set; }
        public IEnumerable<CourseProgressDTO> CompletedCourses { get; set; } = new List<CourseProgressDTO>();
        public IEnumerable<CourseProgressDTO> InProgressCourses { get; set; } = new List<CourseProgressDTO>();
        public IEnumerable<CourseProgressDTO> RemainingCourses { get; set; } = new List<CourseProgressDTO>();
    }

    public class CourseProgressDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public int RecommendedSemester { get; set; }
        public int RecommendedYear { get; set; }
        public bool IsRequired { get; set; }
        public string CourseType { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public string Status { get; set; } = string.Empty; // Completed, InProgress, NotStarted
    }
}
