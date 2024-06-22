using FluentAssertions;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Xunit;

namespace Insurance.Application.Tests;

public class PremiumCalculationTests
{
    [Theory]
    [ClassData(typeof(PremiumCalculationTestData))]
    public void CalculatePremium_ShouldReturnPremium(ComputePremium query, decimal expectedPremium)
    {
        // Arrange & Act
        var premium = PremiumCalculator.Calculate(query);

        // Assert
        premium.Should().Be(expectedPremium);
    }
}
