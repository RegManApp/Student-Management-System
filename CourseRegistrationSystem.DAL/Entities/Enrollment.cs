using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Entities
{
    public class Enrollment
    {
        
        public Status Status {get;set;}
        
        [ForeignKey(nameof(StudentId))]
        public Student StudentId;

        [ForeignKey(nameof(SectionId))]
        public Section SectionId;

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
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Enrollment>()
        .HasKey(e => new { e.StudentId, e.SectionId });

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Student);
        .WithMany(s => s.Enrollments);
        .HasForeignKey(e => e.StudentId);

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Section)
        .WithMany(sec => sec.Enrollments);
        .HasForeignKey(e => e.SectionId);

    base.OnModelCreating(modelBuilder);
}
public enum Status{Enrolled, Dropped, Completed};

public ICollection<Enrollment> Enrollments {get;set;}

