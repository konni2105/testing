using Poc.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<FinancialSimulation> FinancialSimulations { get; set; }
            = new List<FinancialSimulation>();
    }
}