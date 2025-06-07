using AKDashboard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AKDashboard.API.Database
{
    public class ClimaDbContext : DbContext
    {
        public ClimaDbContext(DbContextOptions<ClimaDbContext> options) : base(options) { }

        public DbSet<Clima> Climas { get; set; }
    }
}
