namespace Poc.Application.Features.Simulations.Rules;

public static class SimulationRules
{
    public static void ValidateInputs(
        decimal annualSalary,
        decimal monthlyExpenses,
        decimal savingsGoal)
    {
        if (annualSalary <= 0)
        {
            throw new ArgumentException(
                "Annual salary must be greater than zero.");
        }

        if (monthlyExpenses < 0)
        {
            throw new ArgumentException(
                "Monthly expenses cannot be negative.");
        }

        if (savingsGoal <= 0)
        {
            throw new ArgumentException(
                "Savings goal must be greater than zero.");
        }

        decimal monthlyIncome = annualSalary / 12;

        if (monthlyExpenses >= monthlyIncome)
        {
            throw new ArgumentException(
                "Monthly expenses must be less than monthly income.");
        }
    }

    public static decimal CalculateMonthlySavings(
        decimal annualSalary,
        decimal monthlyExpenses)
    {
        decimal monthlyIncome = annualSalary / 12;

        return Math.Round(
            monthlyIncome - monthlyExpenses, 2);
    }
    public static decimal CalculateAnnualSavings(
        decimal annualSalary,
        decimal monthlyExpenses)
    {
        return Math.Round(
            annualSalary - (monthlyExpenses * 12), 2);
    }

    public static int CalculateMonthsToReachGoal(
        decimal savingsGoal,
        decimal monthlySavings)
    {
        if (monthlySavings <= 0)
        {
            throw new ArgumentException(
                "Monthly savings must be greater than zero.");
        }

        return (int)Math.Ceiling(
            savingsGoal / monthlySavings);
    }
}