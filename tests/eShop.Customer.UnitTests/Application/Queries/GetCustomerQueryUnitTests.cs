using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Queries.GetCustomer;
using eShop.Customer.API.Application.Specifications;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Customer.UnitTests.Application.Queries;

public class GetCustomerQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnSuccessGivenCustomerExists(
        GetCustomerQuery query,
        Domain.AggregatesModel.CustomerAggregate.Customer customer,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomerQueryHandler sut)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(Arg.Any<GetCustomerSpecification>(), default)
            .Returns(customer);

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnNotFoundGivenCustomerDoesNotExist(
        GetCustomerQuery query,
        GetCustomerQueryHandler sut)
    {
        // Arrange

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnErrorWhenExceptionIsThrown(
        GetCustomerQuery query,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomerQueryHandler sut)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(Arg.Any<GetCustomerSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
