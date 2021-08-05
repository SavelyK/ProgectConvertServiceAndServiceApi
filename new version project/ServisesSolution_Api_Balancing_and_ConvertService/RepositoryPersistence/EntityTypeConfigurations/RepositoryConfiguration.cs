
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryDomain;

namespace RepositoryPersistence.EntityTypeConfigurations
{
    public class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
    {
        public void Configure(EntityTypeBuilder<Repository> builder)
        {
           
            builder.HasKey(repository => repository.Id);
            builder.HasIndex(repository => repository.Id).IsUnique();
            builder.Property(repository => repository.Id).ValueGeneratedOnAdd();
            builder.Property(repository => repository.FileName).HasMaxLength(100);
            builder.Property(repository => repository.Path).HasMaxLength(200);
        }
    }
}
