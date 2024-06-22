using FluentAssertions;
using Insurance.Application.DTOs;
using Insurance.ComponentTests.Configuration;
using Insurance.Infrastructure.Repositories.Claims;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Insurance.ComponentTests;

[Collection(nameof(ComponentTestsCollection))]
public class ClaimsTests
{
    private readonly HttpClient _httpClient;

    public ClaimsTests(ComponentTestsFixture componentTestsFixture)
    {
        _httpClient = componentTestsFixture.ApiHttpClient;
    }

    [Fact]
    public async Task GetClaim_WhenExisits_ShouldReturnClaim()
    {
        // Arrange
        var claimId = ClaimsTestData.Claims.First().Id;
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Claims/{claimId}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var claim = JsonConvert.DeserializeObject<ClaimDTO>(content);
        claim.Should().NotBeNull();
    }

    [Fact]
    public async Task GetClaim_WhenNotExisits_ShouldReturnNotFound()
    {
        // Arrange
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Claims/{Guid.NewGuid()}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetClaims_WhenExisits_ShouldReturnList()
    {
        // Arrange
        var limit = 2;
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Claims?offset=0&limit={limit}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var claims = JsonConvert.DeserializeObject<IEnumerable<ClaimDTO>>(content);
        claims.Should().NotBeNull();
        claims!.Count().Should().Be(limit);
    }

    [Fact]
    public async Task CreateClaims_WhenValidData_ShouldCreateClaim()
    {
        // Arrange
        var createdDate = DateTime.UtcNow.AddDays(-5);
        var coverId = CoversTestData.Covers.First(x =>
                x.StartDate < createdDate && x.EndDate > createdDate).Id;
        var claim = new ClaimDTO(string.Empty, coverId, "Test", createdDate,
            ClaimType.Grounding, 100);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Claims");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(claim),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var claimResponse = JsonConvert.DeserializeObject<ClaimDTO>(content);
        claimResponse.Should().NotBeNull();
        claimResponse!.Name.Should().Be(claim.Name);
        claimResponse.CoverId.Should().Be(claim.CoverId);
        claimResponse.Created.Should().Be(claim.Created);
        claimResponse.DamageCost.Should().Be(claim.DamageCost);
        claimResponse.Type.Should().Be(claim.Type);
    }

    [Fact]
    public async Task CreateClaims_WhenClaimNotCovered_ShouldReturnBadRequest()
    {
        // Arrange
        var createdDate = DateTime.UtcNow.AddDays(-5000);
        var coverId = CoversTestData.Covers.First().Id;
        var claim = new ClaimDTO(string.Empty, coverId, "Test", createdDate,
            ClaimType.Grounding, 100);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Claims");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(claim),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateClaims_WhenDamageCostToHigh_ShouldReturnBadRequest()
    {
        // Arrange
        var createdDate = DateTime.UtcNow.AddDays(-5);
        var coverId = CoversTestData.Covers.First().Id;
        var claim = new ClaimDTO(string.Empty, coverId, "Test", createdDate,
            ClaimType.Grounding, 1000001);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,
            $"v1/Claims");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(claim),
            Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
