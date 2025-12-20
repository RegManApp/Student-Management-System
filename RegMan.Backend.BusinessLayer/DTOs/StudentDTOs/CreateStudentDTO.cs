using RegMan.Backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMan.Backend.BusinessLayer.DTOs.StudentDTOs
{
    public class CreateStudentDTO
    {
        //[Required]
        //[StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        //[Required]
        //[StringLength(200)]
        public string? Address { get; set; } = string.Empty;
        public string? FamilyContact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AcademicPlanId { get; set; } = string.Empty;
        
    }
}
