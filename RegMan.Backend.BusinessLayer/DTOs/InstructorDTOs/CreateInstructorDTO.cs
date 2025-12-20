using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.BusinessLayer.DTOs.InstructorDTOs;

public class CreateInstructorDTO
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public InstructorDegree? Degree { get; set; }
    public string? Department { get; set; }
    public string? Address { get; set; }
}
