using Microsoft.EntityFrameworkCore;
using Demo.Models;

namespace Demo.Data
{
    public class AppDbContext : DbContext
    {
        //constructor injection->passes configuration automatically
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //db table
        public DbSet<Employee> Employees { get; set; }
        
        //customize db mapping-> EF calls when automatically when cretate modles

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);
        }
    }
}
