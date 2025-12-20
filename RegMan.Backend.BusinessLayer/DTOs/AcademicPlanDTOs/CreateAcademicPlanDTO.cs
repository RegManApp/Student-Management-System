using System.ComponentModel.DataAnnotations;

namespace RegMan.Backend.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class CreateAcademicPlanDTO
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
