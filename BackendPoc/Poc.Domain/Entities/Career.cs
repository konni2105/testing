using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Domain.Entities
{
  public class Career
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

           public decimal MinimumSalary { get; set; }

        public decimal MaximumSalary { get; set; }

        public int RequiredExperience { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<FinancialSimulation> FinancialSimulations { get; set; }
            = new List<FinancialSimulation>();
    }
}
