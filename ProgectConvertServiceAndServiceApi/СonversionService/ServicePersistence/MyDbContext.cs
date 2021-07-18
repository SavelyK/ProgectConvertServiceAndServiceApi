using LibraryApplication.Interfaces;
using LibraryModels;
using Microsoft.EntityFrameworkCore;
using ServicePersistence.EntityTypeConfigurations;


namespace ServicePersistence
{
    public class MyDbContext : DbContext, IMyDbContext
    {
        private readonly string connectionString;
        public DbSet<DbModel> DbModels { get; set; }
        public MyDbContext()
             : this("Server=localhost;Database=Data58;Trusted_Connection=True;")
        {
            Database.EnsureCreated();
        }
        public MyDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbModelConfiguration());
        }
    }
}
