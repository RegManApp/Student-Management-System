using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.BusinessLayer.DTOs.StudentDTOs
{
    public class UpdateStudentProfileDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = null!;

        //[Required]
        //[StringLength(200)]
        public string? Address { get; set; } = string.Empty;
        public string? FamilyContact { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        //public string AcademicPlanId { get; set; } = string.Empty;
        public int CompletedCredits { get; set; } = 0;
        public int RegisteredCredits { get; set; } = 0;
        public double GPA { get; set; } = 0.0;
    }
}
