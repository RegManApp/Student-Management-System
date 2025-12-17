using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class UpdateAcademicPlanDTO
    {
        [Required]
        public string AcademicPlanId { get; set; } = string.Empty;

        [Required]
        public string MajorName { get; set; } = string.Empty;

        [Required]
        public int TotalCreditsRequired { get; set; }

        public string? Description { get; set; }

        public int ExpectedYearsToComplete { get; set; } = 4;
    }
}
