using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.GetCustomers;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCustomersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerApi customerApi,
        GetCustomersQueryHandler sut,
        CustomerDto[] customers)
    {
        // Arrange

        customerApi.GetCustomers()
            .Returns(customers);

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApi.Received().GetCustomers();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerApi customerApi,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        customerApi.GetCustomers()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApi.Received().GetCustomers();
    }
}
