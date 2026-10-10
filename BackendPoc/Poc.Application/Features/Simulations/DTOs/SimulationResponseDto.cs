namespace Poc.Application.Features.Simulations.DTOs;

public class SimulationResponseDto
{
    public int Id { get; set; }

    public int CareerId { get; set; }

    public decimal AnnualSalary { get; set; }

    public decimal MonthlyExpenses { get; set; }

    public decimal SavingsGoal { get; set; }

    public decimal MonthlySavings { get; set; }

    public decimal AnnualSavings { get; set; }

    public int MonthsToReachGoal { get; set; }
}