﻿using DbInformation.Configuration;
using DbInformation.Interfases;
using DbInformation.Models;
using Microsoft.EntityFrameworkCore;


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