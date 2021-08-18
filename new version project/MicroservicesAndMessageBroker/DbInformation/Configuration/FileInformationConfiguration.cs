using DbInformation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbInformation.Configuration
{
   public class FileInformationConfiguration : IEntityTypeConfiguration<FileInformation>
    {
        public void Configure(EntityTypeBuilder<FileInformation> builder)
        {

            builder.HasKey(file => file.Id);
            builder.Property(file => file.Status).HasMaxLength(100);
            builder.Property(file => file.Path).HasMaxLength(200);
        }
    }
}
