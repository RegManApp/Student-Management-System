using Microsoft.AspNetCore.Identity;

namespace RegMan.Backend.API.Seeders
{
    public static class RoleSeeder
    {
        private static readonly string[] Roles = new[]
        {
            "Admin",
            "Student",
            "Instructor"
        };

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
