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
    }
}
