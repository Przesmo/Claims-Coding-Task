using FluentAssertions;
using Insurance.Application.DTOs;
using Insurance.ComponentTests.Configuration;
using Newtonsoft.Json;
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
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"v1/Claims/{Guid.Empty}");

        // Act
        var response = await _httpClient.SendAsync(requestMessage);

        // Assert
        response.Should().BeSuccessful();
        var content = await response.Content.ReadAsStringAsync();
        var claim = JsonConvert.DeserializeObject<ClaimDTO>(content);
        claim.Should().NotBeNull();
    }
}
