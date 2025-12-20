using Microsoft.AspNetCore.Identity;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.API.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(UserManager<BaseUser> userManager)
        {
            var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("ADMIN_EMAIL environment variable is not configured.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("ADMIN_PASSWORD environment variable is not configured.");

            var admin = await userManager.FindByEmailAsync(email);

            if (admin != null)
                return;

            admin = new BaseUser
            {
                UserName = email,
                Email = email,
                FullName = "System Admin",
                Address = "HQ",
                Role = "Admin"
            };

            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user: {errors}");
            }

            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
