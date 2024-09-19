using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Queries.GetCustomers;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Customer.UnitTests.Application.Queries;

public class GetCustomersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnCustomers(
        GetCustomersQuery query,
        List<Domain.AggregatesModel.CustomerAggregate.Customer> customers,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        customerRepository.ListAsync(default).Returns(customers);

        // Act

        Result<List<CustomerDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(customers.Count, result.Value.Count);
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnNotFoundGivenNoCustomers(
        GetCustomersQuery query,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        // Act

        Result<List<CustomerDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsNotFound());

        await customerRepository.Received().ListAsync(default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnErrorWhenExceptionIsThrown(
        GetCustomersQuery query,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        customerRepository.ListAsync(default).ThrowsAsync<Exception>();

        // Act

        Result<List<CustomerDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError());

        await customerRepository.Received().ListAsync(default);
    }
}
