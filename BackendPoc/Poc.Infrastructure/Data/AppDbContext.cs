using Microsoft.EntityFrameworkCore;
using Poc.Application.Common.Interfaces;
using Poc.Domain.Entities;

namespace Poc.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Career> Careers { get; set; }

        public DbSet<FinancialSimulation> FinancialSimulations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Career>()
                .Property(c => c.MinimumSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Career>()
                .Property(c => c.MaximumSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FinancialSimulation>()
                .Property(s => s.AnnualSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FinancialSimulation>()
                .Property(s => s.MonthlyExpenses)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FinancialSimulation>()
                .Property(s => s.SavingsGoal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FinancialSimulation>()
                .Property(s => s.MonthlySavings)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FinancialSimulation>()
                .Property(s => s.AnnualSavings)
                .HasPrecision(18, 2);
        }
    }
}