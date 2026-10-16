using Microsoft.EntityFrameworkCore;
using Poc.Application.Common.Interfaces;
using Poc.Application.Features.Simulations.DTOs;
using Poc.Application.Features.Simulations.Services;
using Poc.Infrastructure.Data;
using Xunit;

namespace Poc.Tests.Unit;

public class UnitTest1
{
    [Fact]
    public async Task CreateSimulationAsync_CareerNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new AppDbContext(options);

        IAppDbContext context = dbContext;
        var service = new SimulationService(context);

        var request = new CreateSimulationDto
        {
            CareerId = 999,
            AnnualSalary = 600000m,
            MonthlyExpenses = 25000m,
            SavingsGoal = 100000m
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.CreateSimulationAsync(request, userId: 1));
    }
}