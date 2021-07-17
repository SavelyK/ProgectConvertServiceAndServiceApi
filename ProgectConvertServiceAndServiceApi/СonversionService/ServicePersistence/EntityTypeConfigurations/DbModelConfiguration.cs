using LibraryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ServicePersistence.EntityTypeConfigurations
{
    public class DbModelConfiguration : IEntityTypeConfiguration<DbModel>
    {
        public void Configure(EntityTypeBuilder<DbModel> builder)
        {
            builder.ToTable("DbModel");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.FileName).HasMaxLength(100);
            builder.Property(s => s.Path).HasMaxLength(200);
        }

    }
}
