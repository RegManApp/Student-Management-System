using System.ComponentModel.DataAnnotations;

namespace RegMan.Backend.BusinessLayer.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        // Role is optional for public registration (defaults to Student)
        // Admin uses different endpoint to create users with specific roles
        public string? Role { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
