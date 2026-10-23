using GymCoreProject.Configurations;
using GymCoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GymCoreProject.DbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymManagementC47;Trusted_Connection=True; TrustServerCertificate = true");
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
