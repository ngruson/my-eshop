using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Commands.DeleteCustomer;
using eShop.Customer.API.Application.Specifications;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Customer.UnitTests.Application.Commands;

public class DeleteCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCustomerDeleted(
        DeleteCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        DeleteCustomerCommandHandler sut,
        Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(
                Arg.Any<GetCustomerByObjectIdSpecification>(),
                default)
            .Returns(customer);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerRepository.Received().UpdateAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenCustomerDoesNotExist(
        DeleteCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());

        await customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(
                Arg.Any<GetCustomerByObjectIdSpecification>(),
                default)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }
}
