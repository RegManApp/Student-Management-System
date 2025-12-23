using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RegMan.Backend.DAL.DataContext
{
    public class AppDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // اقرأ appsettings.json من مشروع API
            var configuration = new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "../RegMan.Backend.API"
                    )
                )
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            }

            // Design-time migrations don't need a real DB connection, but the provider needs a non-empty string.
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=RegMan.DesignTime;Trusted_Connection=True;MultipleActiveResultSets=true";
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
