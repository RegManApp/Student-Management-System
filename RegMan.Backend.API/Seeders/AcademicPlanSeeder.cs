using Microsoft.EntityFrameworkCore;
using RegMan.Backend.DAL.DataContext;
using RegMan.Backend.DAL.Entities;

namespace RegMan.Backend.API.Seeders
{
    public static class AcademicPlanSeeder
    {
        public static async Task SeedDefaultAcademicPlanAsync(AppDbContext context)
        {
            // Check if any academic plan exists
            if (await context.AcademicPlans.AnyAsync())
            {
                return;
            }

            // Seed default academic plan
            var defaultPlan = new AcademicPlan
            {
                AcademicPlanId = "default",
                MajorName = "Undeclared",
                TotalCreditsRequired = 120,
                Description = "Default academic plan for new students",
                ExpectedYearsToComplete = 4,
                CreatedAt = DateTime.UtcNow
            };

            context.AcademicPlans.Add(defaultPlan);
            await context.SaveChangesAsync();
        }
    }
}
