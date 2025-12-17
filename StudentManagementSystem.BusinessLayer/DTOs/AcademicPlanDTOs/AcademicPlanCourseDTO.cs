using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class AddCourseToAcademicPlanDTO
    {
        [Required]
        public string AcademicPlanId { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }

        public int RecommendedSemester { get; set; } // 1-8

        public int RecommendedYear { get; set; } // 1-4

        public bool IsRequired { get; set; } = true;

        public int CourseTypeId { get; set; } = 0; // Maps to CourseType enum
    }

    public class AcademicPlanCourseDTO
    {
        public int AcademicPlanCourseId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public int RecommendedSemester { get; set; }
        public int RecommendedYear { get; set; }
        public bool IsRequired { get; set; }
        public string CourseType { get; set; } = string.Empty;
    }
}
