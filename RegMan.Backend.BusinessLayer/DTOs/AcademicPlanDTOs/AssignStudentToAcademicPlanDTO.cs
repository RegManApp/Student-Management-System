using System.ComponentModel.DataAnnotations;

namespace RegMan.Backend.BusinessLayer.DTOs.AcademicPlanDTOs
{
    public class AssignStudentToAcademicPlanDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public string AcademicPlanId { get; set; } = string.Empty;
    }
}
