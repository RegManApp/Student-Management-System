using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationSystem.DAL.Entities;

public abstract class User
{
    [Key] protected int UserId { get; set; } = 0;

    [Required] protected string Username { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] protected string FullName { get; set; } = string.Empty;

    [Required] [EmailAddress] protected string Email { get; set; } = string.Empty;

    [Required] protected string Role { get; set; } = string.Empty;

    public bool Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            return false;

        if (!string.Equals(Username, username, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(Email, username, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(Password, password))
            return false;
        
        return true;
    }
    
    public void Logout()
    {
    }
}