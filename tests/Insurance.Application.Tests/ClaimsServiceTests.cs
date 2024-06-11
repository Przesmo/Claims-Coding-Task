using FluentAssertions;
using Insurance.Application.Services;
using Insurance.Host.Messages.Commands;
using Insurance.Infrastructure.Repositories.Claims;
using Moq;
using Xunit;

namespace Insurance.Application.Tests;

public class ClaimsServiceTests
{
    private readonly IClaimsService _claimsService;
    private readonly Mock<IClaimsRepository> _claimsRepositoryMock = new();
    public ClaimsServiceTests()
    {
        _claimsService = new ClaimsService(_claimsRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateNewClaim()
    {
        // Arrange
        var command = new CreateClaim
        {
            CoverId = "123",
            Created = DateTime.UtcNow,
            DamageCost = 1,
            Name = "Name",
            Type = ClaimType.Fire
        };
        _claimsRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Claim>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _claimsService.CreateAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.CoverId.Should().Be(command.CoverId);
        result.Created.Should().Be(command.Created);
        result.DamageCost.Should().Be(command.DamageCost);
        result.Name.Should().Be(command.Name);
        result.Type.Should().Be(command.Type);
    }
}
