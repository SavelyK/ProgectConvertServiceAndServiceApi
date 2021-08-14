using ConvertService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertService
{
    public class ApplicationContext : DbContext
    {
        public DbSet<DocxItemModel> DocxItemModels { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DocxItemModelConfiguration());
            base.OnModelCreating(builder);
        }

    }
}
