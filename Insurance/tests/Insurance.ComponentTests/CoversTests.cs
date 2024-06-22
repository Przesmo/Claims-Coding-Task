using FluentAssertions;
using Insurance.Application.DTOs;
using Insurance.Application.Messages.Commands;
using Insurance.ComponentTests.Configuration;
using Insurance.Infrastructure.Repositories.Covers;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Insurance.ComponentTests;

[Collection(nameof(ComponentTestsCollection))]
public class CoversTests
{
    private readonly HttpClient _httpClient;

    public CoversTests(ComponentTestsFixture componentTestsFixture)
    {
        _httpClient = componentTestsFixture.ApiHttpClient;
    }

    [Fact]
    public async Task GetCover_WhenExisits_ShouldReturnCover()
    {
        // Arrange
        var coverId = CoversTestData.Covers.First().Id;
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Covers/{coverId}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var cover = JsonConvert.DeserializeObject<CoverDTO>(content);
        cover.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCover_WhenNotExisits_ShouldReturnNoContent()
    {
        // Arrange
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Covers/{Guid.NewGuid()}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetCovers_WhenExisits_ShouldReturnList()
    {
        // Arrange
        var limit = 2;
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Covers?offset=0&limit={limit}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var covers = JsonConvert.DeserializeObject<IEnumerable<CoverDTO>>(content);
        covers.Should().NotBeNull();
        covers!.Count().Should().Be(limit);
    }

    [Fact]
    public async Task DeleteCover_WhenExisits_ShouldReturnRemove()
    {
        // Arrange
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
            $"v1/Covers/{CoversTestData.CoverToDeleteId}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task CreateCover_WhenValidData_ShouldCreateCover()
    {
        // Arrange
        var cover = new CreateCover
        {
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10),
            Type = CoverType.Tanker
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Covers");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(cover),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var coverResponse = JsonConvert.DeserializeObject<CoverDTO>(content);
        coverResponse.Should().NotBeNull();
        coverResponse!.Type.Should().Be(cover.Type);
        coverResponse.StartDate.Should().Be(cover.StartDate);
        coverResponse.EndDate.Should().Be(cover.EndDate);
    }

    [Fact]
    public async Task CreateCover_WhenStartDateInThePast_ShouldReturnBadRequest()
    {
        // Arrange
        var cover = new CreateCover
        {
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(10),
            Type = CoverType.Tanker
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Covers");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(cover),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateCover_WhenInsurancePeriodExceeds_ShouldReturnBadRequest()
    {
        // Arrange
        var cover = new CreateCover
        {
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(370),
            Type = CoverType.Tanker
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Covers");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(cover),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
