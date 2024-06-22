using FluentAssertions;
using Insurance.Application.DTOs;
using Insurance.ComponentTests.Configuration;
using Newtonsoft.Json;
using System.Net;
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
}
