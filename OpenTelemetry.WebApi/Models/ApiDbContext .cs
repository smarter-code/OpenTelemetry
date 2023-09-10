using Microsoft.EntityFrameworkCore;

namespace OpenTelemetry.WebApi.Models
{
    public class ApiDbContext : DbContext
    {
        public DbSet<ApiResponse> ApiResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=apiresponses.db");
        }
    }
}
