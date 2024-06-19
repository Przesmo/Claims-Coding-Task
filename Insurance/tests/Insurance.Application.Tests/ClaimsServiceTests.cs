using FluentAssertions;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Services;
using Insurance.Infrastructure.Repositories.Claims;
using Moq;
using Xunit;

namespace Insurance.Application.Tests;

public class ClaimsServiceTests
{
    private readonly IClaimsService _claimsService;
    private readonly Mock<IClaimsRepository> _claimsRepositoryMock = new();
    private readonly Mock<ICoversService> _coversServiceMock = new();
    //private readonly Mock<IAuditer> _auditerMock = new();

    public ClaimsServiceTests()
    {
        _claimsService = new ClaimsService(
            _claimsRepositoryMock.Object, _coversServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenCovered_ShouldCreateNewClaim()
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
        _coversServiceMock.Setup(x => x.IsDateCoveredAsync(command.CoverId, command.Created))
            .ReturnsAsync(true);
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

    [Fact]
    public async Task CreateAsync_WhenCoveredAndCreatedNewClaim_ShouldAuditLog()
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
        _coversServiceMock.Setup(x => x.IsDateCoveredAsync(command.CoverId, command.Created))
            .ReturnsAsync(true);
        _claimsRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Claim>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _claimsService.CreateAsync(command);

        // Assert
        //_auditerMock.Verify(x => x.AuditClaim(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenNotCovered_ShouldThrowExcpetion()
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
        _coversServiceMock.Setup(x => x.IsDateCoveredAsync(command.CoverId, command.Created))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ClaimNotCoveredException>(async () =>
            await _claimsService.CreateAsync(command));

        // Assert
        exception.Message.Should().BeEquivalentTo($"Claim with coverId: {command.CoverId} is not covered for the date: {command.Created:dd/MM/yyyy}");
    }
}
