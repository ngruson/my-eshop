using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Customer.CreateCustomer;
using eShop.Customer.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class CreateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApi.Received().CreateCustomer(command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        customerApi.CreateCustomer(command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApi.Received().CreateCustomer(command.Dto);
    }
}
