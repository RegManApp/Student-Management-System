namespace RegMan.Backend.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class ViewAcademicPlanDTO
    {
        public string AcademicPlanId { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public int TotalCreditsRequired { get; set; }
        public string? Description { get; set; }
        public int ExpectedYearsToComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalStudents { get; set; }
        public IEnumerable<AcademicPlanCourseDTO> Courses { get; set; } = new List<AcademicPlanCourseDTO>();
    }

    public class ViewAcademicPlanSummaryDTO
    {
        public string AcademicPlanId { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public int TotalCreditsRequired { get; set; }
        public int ExpectedYearsToComplete { get; set; }
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
    }
}
