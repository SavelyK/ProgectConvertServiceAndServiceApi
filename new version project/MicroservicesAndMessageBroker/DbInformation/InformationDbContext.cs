using DbInformation.Configuration;
using DbInformation.Interfases;
using DbInformation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbInformation
{
    public class InformationDbContext : DbContext, IInformationDbContext
    {
        public DbSet<FileInformation> FileInformations { get; set; }
        public InformationDbContext(DbContextOptions<InformationDbContext> options)
             : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileInformationConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
