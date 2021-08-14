
using ConvertService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ConvertService
{
    public class DocxItemModelConfiguration : IEntityTypeConfiguration<DocxItemModel>
    {
        public void Configure(EntityTypeBuilder<DocxItemModel> builder)
        {
           
            builder.HasKey(docxItem => docxItem.Id);
            builder.Property(docxItem => docxItem.Status).HasMaxLength(100);
            builder.Property(docxItem => docxItem.Path).HasMaxLength(200);
        }
    }
}
