using Auditing.Host.Contracts;
using FluentAssertions;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Insurance.Infrastructure.AuditingIntegration;
using Insurance.Infrastructure.Repositories.Covers;
using Moq;
using Xunit;

namespace Insurance.Application.Tests;

public class CoversServiceTests
{
    private readonly ICoversService _coversService;
    private readonly Mock<ICoversRepository> _coversRepositoryMock = new();
    private readonly Mock<IAuditingQueue> _auditingQueueMock = new();

    public CoversServiceTests()
    {
        _coversService = new CoversService(_coversRepositoryMock.Object,
            _auditingQueueMock.Object);
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
        var query = new IsDateCovered { CoverId = cover.Id, DateToCover = DateTime.UtcNow.AddDays(-5) };
        // Act
        var result = await _coversService.IsDateCoveredAsync(query);

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
        var query = new IsDateCovered { CoverId = coverId, DateToCover = DateTime.UtcNow.AddDays(-5) };

        // Act
        var result = await _coversService.IsDateCoveredAsync(query);

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
        var query = new IsDateCovered { CoverId = cover.Id, DateToCover = DateTime.UtcNow.AddDays(-5) };

        // Act
        var result = await _coversService.IsDateCoveredAsync(query);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenCoverIsRemoved_ShouldAuditLog()
    {
        // Arrange
        var command = new DeleteCover
        {
            Id = "123"
        };
        _coversRepositoryMock.Setup(x => x.DeleteAsync(command.Id))
            .Returns(Task.CompletedTask);

        // Act
        await _coversService.DeleteAsync(command);

        // Assert
        _auditingQueueMock.Verify(x => x.PublishAsync(It.IsAny<AddAuditLog>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCoverIsCreated_ShouldAuditLog()
    {
        // Arrange
        var command = new CreateCover
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10)
        };
        _coversRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Cover>()))
            .Returns(Task.CompletedTask);

        // Act
        await _coversService.CreateAsync(command);

        // Assert
        _auditingQueueMock.Verify(x => x.PublishAsync(It.IsAny<AddAuditLog>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenInsurancePeriodExceeded_ShouldThrowException()
    {
        // Arrange
        var command = new CreateCover
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(400)
        };

        // Act
        var exception = await Assert.ThrowsAsync<InsurancePeriodExceededException>(async () =>
            await _coversService.CreateAsync(command));

        // Assert
        exception.Message.Should().BeEquivalentTo("Maximum insurance period of 365 days has been exceeded");
    }
}
