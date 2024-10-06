using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Commands.CreateCustomer;
using eShop.Customer.Domain.AggregatesModel.CustomerAggregate;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Customer.UnitTests.Application.Commands;

public class CreateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnSuccessGivenCustomerCreated(
        CreateCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(
            command with { Dto = command.Dto with { CardType = CardType.Amex.Name } },
            CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerRepository.Received().AddAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnErrorWhenExceptionIsThrown(
        CreateCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        CreateCustomerCommandHandler sut)
    {
        // Arrange        

        customerRepository.AddAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(
            command with { Dto = command.Dto with { CardType = CardType.Amex.Name } }
            , CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerRepository.Received().AddAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }
}
