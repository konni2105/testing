
using Poc.Application.Features.Simulations.DTOs;
using Poc.Application.Features.Simulations.Interfaces;
using Poc.Application.Features.Simulations.Rules;

namespace Poc.Application.Features.Simulations.Services;

public class SimulationService : ISimulationService
{
    public Task<SimulationResponseDto> CreateSimulationAsync(
        CreateSimulationDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        SimulationRules.ValidateInputs(
            request.AnnualSalary,
            request.MonthlyExpenses,
            request.SavingsGoal);

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

        var response = new SimulationResponseDto
        {
            CareerId = request.CareerId,
            AnnualSalary = request.AnnualSalary,
            MonthlyExpenses = request.MonthlyExpenses,
            SavingsGoal = request.SavingsGoal,
            MonthlySavings = monthlySavings,
            AnnualSavings = annualSavings,
            MonthsToReachGoal = monthsToReachGoal
        };

        return Task.FromResult(response);
    }
}
