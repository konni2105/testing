
using Poc.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Domain.Entities
{
    public class FinancialSimulation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CareerId { get; set; }

        public decimal AnnualSalary { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public decimal SavingsGoal { get; set; }

        public decimal MonthlySavings { get; set; }

        public decimal AnnualSavings { get; set; }

        public SimulationStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public User User { get; set; } = null!;

        public Career Career { get; set; } = null!;
    }
}
