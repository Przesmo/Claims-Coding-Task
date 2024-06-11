using FluentAssertions;
using Insurance.Application.Services;
using Insurance.Infrastructure.Repositories.Covers;
using Moq;
using Xunit;

namespace Insurance.Application.Tests;

public class CoversServiceTests
{
    private readonly ICoversService _coversService;
    private readonly Mock<ICoversRepository> _coversRepositoryMock = new();
    public CoversServiceTests()
    {
        _coversService = new CoversService(_coversRepositoryMock.Object);
    }

    [Fact]
    public async Task IsDateCoveredAsync_WhenCovered_ShouldReturnTrue()
    {
        // Arrange
        var cover = new Cover
        {
            Id = "10",
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        _coversRepositoryMock.Setup(x => x.GetAsync(cover.Id))
            .ReturnsAsync(cover);

        // Act
        var result = await _coversService.IsDateCoveredAsync(cover.Id, DateTime.UtcNow.AddDays(-5));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsDateCoveredAsync_WhenCoverDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var coverId = "10";
        _coversRepositoryMock.Setup(x => x.GetAsync(coverId))
            .ReturnsAsync((Cover?)null);

        // Act
        var result = await _coversService.IsDateCoveredAsync(coverId, DateTime.UtcNow.AddDays(-5));

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsDateCoveredAsync_WhenIsNotCovered_ShouldReturnFalse()
    {
        // Arrange
        var cover = new Cover
        {
            Id = "10",
            StartDate = DateTime.UtcNow.AddDays(-15),
            EndDate = DateTime.UtcNow.AddDays(-10)
        };
        _coversRepositoryMock.Setup(x => x.GetAsync(cover.Id))
            .ReturnsAsync(cover);

        // Act
        var result = await _coversService.IsDateCoveredAsync(cover.Id, DateTime.UtcNow.AddDays(-5));

        // Assert
        result.Should().BeFalse();
    }
}
