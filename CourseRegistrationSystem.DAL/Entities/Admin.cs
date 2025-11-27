using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DAL.Entities;

public class Admin : User
{
    [Required] private string Title { get; set; } = string.Empty;

    public void CreateCourse(Course course)
    {
    }

    public void UpdateCourse(Course course)
    {
    }

    public void DeleteCourse(Course course)
    {
    }
}