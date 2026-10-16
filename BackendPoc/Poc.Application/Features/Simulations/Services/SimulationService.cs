
using Microsoft.EntityFrameworkCore;
using Poc.Application.Common.Interfaces;
using Poc.Application.Features.Simulations.DTOs;
using Poc.Application.Features.Simulations.Interfaces;
using Poc.Application.Features.Simulations.Rules;
using Poc.Domain.Entities;
using Poc.Domain.Enum;

namespace Poc.Application.Features.Simulations.Services;

public class SimulationService : ISimulationService
{
    private readonly IAppDbContext _context;

    public SimulationService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<SimulationResponseDto> CreateSimulationAsync(
        CreateSimulationDto request,
        int userId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // 1. Validate financial inputs
        SimulationRules.ValidateInputs(
            request.AnnualSalary,
            request.MonthlyExpenses,
            request.SavingsGoal);

        // 2. Validate career
        var career = await _context.Careers
            .FirstOrDefaultAsync(
                c => c.Id == request.CareerId && c.IsActive,
                cancellationToken);

        if (career is null)
        {
            throw new KeyNotFoundException(
                $"Career with ID {request.CareerId} was not found or is inactive.");
        }

        // 3. Calculate financial results
        var monthlySavings = SimulationRules.CalculateMonthlySavings(
            request.AnnualSalary,
            request.MonthlyExpenses);

        var annualSavings = SimulationRules.CalculateAnnualSavings(
            request.AnnualSalary,
            request.MonthlyExpenses);

        var monthsToReachGoal =
            SimulationRules.CalculateMonthsToReachGoal(
                request.SavingsGoal,
                monthlySavings);

        // 4. Create simulation entity
        var simulation = new FinancialSimulation
        {
            UserId = userId,
            CareerId = request.CareerId,
            AnnualSalary = request.AnnualSalary,
            MonthlyExpenses = request.MonthlyExpenses,
            SavingsGoal = request.SavingsGoal,
            MonthlySavings = monthlySavings,
            AnnualSavings = annualSavings,
            Status = annualSavings >= request.SavingsGoal
                ? SimulationStatus.GoalAchieved
                : SimulationStatus.GoalNotAchieved,
            CreatedDate = DateTime.UtcNow
        };

        // 5. Save simulation to database
        await _context.FinancialSimulations.AddAsync(
            simulation, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        // 6. Return response
        return new SimulationResponseDto
        {
            CareerId = request.CareerId,
            AnnualSalary = request.AnnualSalary,
            MonthlyExpenses = request.MonthlyExpenses,
            SavingsGoal = request.SavingsGoal,
            MonthlySavings = monthlySavings,
            AnnualSavings = annualSavings,
            MonthsToReachGoal = monthsToReachGoal
        };
    }
}
