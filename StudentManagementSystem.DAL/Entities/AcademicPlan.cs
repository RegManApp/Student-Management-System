using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Entities
{
    public class AcademicPlan
    {
        [Key]
        public string AcademicPlanId { get; set; }
        public string MajorName { get; set; }
        public int Credits { get; set; }

        public ICollection<Course> Courses = new HashSet<Course>();

   
    }
}





