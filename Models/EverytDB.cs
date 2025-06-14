using Microsoft.EntityFrameworkCore;

namespace Carpenter.Models
{
    public class EverytDB : DbContext
    {
        
        public DbSet<AllProducts> AllProductss { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WorkProject> WorkProjects { get; set; }
        public DbSet<ProjectItem> ProjectItems { get; set; }



        public EverytDB(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-5L2MAI1U\\CARPENTERDBSQL;Database=CarpenterDB;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set default values explicitly in the database schema
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Photo)
                .HasDefaultValue("/Media/Images/UserProfile/defaultuser.png");

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Country)
                .HasDefaultValue("Earth");

            base.OnModelCreating(modelBuilder);
        }
    }
}
