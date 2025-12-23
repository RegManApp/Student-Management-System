using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.DTOs.CourseDTOs
{
    public class ViewCourseSummaryDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public string CourseCode { get; set; } = string.Empty;

        public int CourseCategoryId { get; set; }
        public string CourseCategoryName => ((CourseCategory)CourseCategoryId).ToString();

        public string? Description { get; set; }
    }
}
