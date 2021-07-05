using ConversionService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConversionService
{
    public class MyDbContext : DbContext
    {
        private readonly string connectionString;
        
        public DbSet<DbModel> DbModels { get; set; }
        public MyDbContext()
             : this("Server=localhost;Database=Data22;Trusted_Connection=True;")
        {
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
