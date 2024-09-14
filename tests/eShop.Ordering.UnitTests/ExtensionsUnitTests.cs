using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Ordering.UnitTests;

public class ExtensionsUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task test(
        WebApplicationFactory<Program> factory)
    {
        // Arrange

        HttpClient client = factory.CreateClient();

        // Act

        HttpResponseMessage httpResponseMessage = await client.GetAsync("/");

        // Assert

    }
}
