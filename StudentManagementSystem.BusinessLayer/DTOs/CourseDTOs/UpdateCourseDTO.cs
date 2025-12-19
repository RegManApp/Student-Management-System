using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.BusinessLayer.DTOs.CourseDTOs
{
    public class UpdateCourseDTO
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string CourseName { get; set; } = string.Empty;
        [Required]
        [Range(1, 4)]
        public int CreditHours { get; set; }
        //[Required]
        //[Range(30, 60)]
        //public int AvailableSeats { get; set; }
        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string CourseCode { get; set; } = string.Empty;
        [Required]
        public int CourseCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
