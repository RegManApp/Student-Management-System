using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DAL.Entities;

public class Instructor : User
{
    [Required] private string Title { get; set; } = string.Empty;
    [Required] private List<Section> Sections { get; set; } = new();

    public bool AssignGrade(Section section, Student student, string grade)
    {
        return true;
    }
}