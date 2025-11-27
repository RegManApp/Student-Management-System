using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DAL.Entities;

public class Student : User
{
    private string Address { get; set; } = string.Empty;
    private string PhoneNumber { get; set; } = string.Empty;
    [Required] private AcademicPlan AcademicPlan { get; set; } = new AcademicPlan();
    [Required] private string FamilyContact { get; set; } = string.Empty;
    private double GPA { get; set; } = 0.0;
    private int CompletedCredits { get; set; } = 0;
    private int RegisteredCredits { get; set; } = 0;

    public bool RegisterSection(Section section)
    {
        return true;
    }

    public bool DropSection(Section section)
    {
        return true;
    }

    public Transcript ViewTranscript()
    {
        return new Transcript();
    }

    public double CalculateGPA()
    {
        return 0.0;
    }
}