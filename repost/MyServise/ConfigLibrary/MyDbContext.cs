using LibraryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigLibrary
{
    public class MyDbContext : DbContext
    {
        private readonly string connectionString;
        public DbSet<DbModel> DbModels { get; set; }
        public MyDbContext()
             : this("Server=localhost;Database=Data12;Trusted_Connection=True;")
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
