using Microsoft.EntityFrameworkCore;
using RepositoryApplication.Interfases;
using RepositoryDomain;
using RepositoryPersistence.EntityTypeConfigurations;

namespace RepositoryPersistence
{
    public class RepositoryDbContext : DbContext, IRepositoryDbContext
    {
        public DbSet<Repository> Repositorys { get; set; }
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
             : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RepositoryConfiguration());
            base.OnModelCreating(builder);
        }

        private readonly string connectionString;


       
        public RepositoryDbContext()
             : this("Server=localhost;Database=Data68;Trusted_Connection=True;")
        {

        }
        public RepositoryDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Data68;Trusted_Connection=True;");
        }


    }
}
