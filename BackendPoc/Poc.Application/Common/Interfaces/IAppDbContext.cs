using Microsoft.EntityFrameworkCore;
using Poc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }

        DbSet<Career> Careers { get; }

        DbSet<FinancialSimulation> FinancialSimulations{ get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
