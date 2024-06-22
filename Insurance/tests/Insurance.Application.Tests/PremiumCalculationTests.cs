using FluentAssertions;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Insurance.Infrastructure.Repositories.Covers;
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

    [Fact]
    public void CalculatePremium_WhenInsuranceLengthExceeded_ShouldThrowException()
    {
        // Arrange
        var query = new ComputePremium 
        {
            CoverType = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(400)
        };

        // Act
        var exception = Assert.Throws<InsurancePeriodExceededException>(() =>
            PremiumCalculator.Calculate(query));

        // Assert
        exception.Message.Should().BeEquivalentTo("Maximum insurance period of 365 days has been exceeded");
    }
}
