namespace Poc.Application.Features.Simulations.DTOs;

public class CreateSimulationDto
{
    public int CareerId { get; set; }

    public decimal AnnualSalary { get; set; }

    public decimal MonthlyExpenses { get; set; }

    public decimal SavingsGoal { get; set; }
}