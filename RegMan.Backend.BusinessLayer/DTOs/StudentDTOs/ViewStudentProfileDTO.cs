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
    public class ViewStudentProfileDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;

        public string FamilyContact { get; set; } = null!;
        public int CompletedCredits { get; set; } = 0;
        public int RegisteredCredits { get; set; } = 0;
        public int RemainingCredits { get; set; }
        public double GPA { get; set; } = 0.0;
        public string UserId { get; set; } = null!;
        public string AcademicPlanId { get; set; } = null!;
    }
}
