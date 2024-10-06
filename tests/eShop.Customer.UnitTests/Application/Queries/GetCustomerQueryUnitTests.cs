using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Queries.GetCustomerByObjectId;
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
        GetCustomerByObjectIdQuery query,
        Domain.AggregatesModel.CustomerAggregate.Customer customer,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomerByObjectIdQueryHandler sut)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(Arg.Any<GetCustomerByObjectIdSpecification>(), default)
            .Returns(customer);

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnNotFoundGivenCustomerDoesNotExist(
        GetCustomerByObjectIdQuery query,
        GetCustomerByObjectIdQueryHandler sut)
    {
        // Arrange

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnErrorWhenExceptionIsThrown(
        GetCustomerByObjectIdQuery query,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        GetCustomerByObjectIdQueryHandler sut)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(Arg.Any<GetCustomerByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CustomerDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
