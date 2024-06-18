using Insurance.ComponentTests.Configuration;
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
    public void Valid()
    {
        Assert.Equal(1, 1);
    }
}
