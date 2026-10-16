
using Poc.Application.Features.Simulations.Rules;

namespace Poc.Tests.Unit;

public class SimulationRulesTests
{
    [Fact]
    public void CalculateMonthlySavings_ValidInputs_ReturnsExpectedAmount()
    {
        // Arrange
        decimal annualSalary = 600000m;
        decimal monthlyExpenses = 25000m;

        // Act
        decimal result = SimulationRules.CalculateMonthlySavings(
            annualSalary, monthlyExpenses);

        // Assert
        Assert.Equal(25000m, result);
    }

    [Fact]
    public void CalculateAnnualSavings_ValidInputs_ReturnsExpectedAmount()
    {
        // Arrange
        decimal annualSalary = 600000m;
        decimal monthlyExpenses = 25000m;

        // Act
        decimal result = SimulationRules.CalculateAnnualSavings(
            annualSalary, monthlyExpenses);

        // Assert
        Assert.Equal(300000m, result);
    }

    [Fact]
    public void CalculateMonthsToReachGoal_ValidInputs_ReturnsExpectedMonths()
    {
        // Arrange
        decimal savingsGoal = 100000m;
        decimal monthlySavings = 25000m;

        // Act
        int result = SimulationRules.CalculateMonthsToReachGoal(
            savingsGoal, monthlySavings);

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void CalculateMonthsToReachGoal_PartialMonth_RoundsUp()
    {
        // Arrange
        decimal savingsGoal = 100001m;
        decimal monthlySavings = 25000m;

        // Act
        int result = SimulationRules.CalculateMonthsToReachGoal(
            savingsGoal, monthlySavings);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ValidateInputs_ValidInputs_DoesNotThrow()
    {
        // Arrange
        decimal annualSalary = 600000m;
        decimal monthlyExpenses = 25000m;
        decimal savingsGoal = 100000m;

        // Act and Assert
        var exception = Record.Exception(() =>
            SimulationRules.ValidateInputs(
                annualSalary, monthlyExpenses, savingsGoal));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateInputs_InvalidSalary_ThrowsArgumentException(
        decimal annualSalary)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            SimulationRules.ValidateInputs(
                annualSalary, 10000m, 50000m));

        Assert.Equal(
            "Annual salary must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void ValidateInputs_NegativeExpenses_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            SimulationRules.ValidateInputs(
                600000m, -1m, 50000m));

        Assert.Equal(
            "Monthly expenses cannot be negative.",
            exception.Message);
    }

    [Fact]
    public void ValidateInputs_ZeroSavingsGoal_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            SimulationRules.ValidateInputs(
                600000m, 10000m, 0m));

        Assert.Equal(
            "Savings goal must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void ValidateInputs_ExpensesEqualMonthlyIncome_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            SimulationRules.ValidateInputs(
                600000m, 50000m, 100000m));

        Assert.Equal(
            "Monthly expenses must be less than monthly income.",
            exception.Message);
    }

    [Fact]
    public void CalculateMonthsToReachGoal_ZeroMonthlySavings_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            SimulationRules.CalculateMonthsToReachGoal(
                100000m, 0m));

        Assert.Equal(
            "Monthly savings must be greater than zero.",
            exception.Message);
    }
}
