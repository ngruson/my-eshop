using eShop.Customer.API;
using Microsoft.AspNetCore.Routing;

namespace eShop.Customer.UnitTests;

public class CustomerApiUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void success(
            IEndpointRouteBuilder routeBuilder)
    {
        // Arrange

        // Act

        routeBuilder = routeBuilder.MapCustomerApiV1();

        // Assert

        Assert.Single(routeBuilder.DataSources);
    }
}
