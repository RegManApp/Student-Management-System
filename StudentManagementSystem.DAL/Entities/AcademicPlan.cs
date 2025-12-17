using StudentManagementSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.DAL.Entities
{
    public class AcademicPlan
    {
        [Key]
        public string AcademicPlanId { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public StudentProfile Student { get; set; } = null!;


    }
}





