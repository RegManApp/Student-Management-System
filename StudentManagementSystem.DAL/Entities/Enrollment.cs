using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public Status Status {get;set;}
        
        [ForeignKey("Student")]
        public int StudentId{ get; set; }

        [ForeignKey("Section")]
        public int SectionId{ get; set; }

        // Operations
        
        public void ChangeStatus(Status newStatus)
        {
            Status = newStatus;
        }


        public bool IsActive()
        {
            return Status == Status.Enrolled;
        }


    public void Drop()
    {
        Status = Status.Dropped;
    }

    }
}

public enum Status{Enrolled, Dropped, Completed};
