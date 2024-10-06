using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using eShop.AdminApp.Application.Commands.Customer.UpdateCustomer;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class UpdateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerUpdated(
        UpdateCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        UpdateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApi.Received().UpdateCustomer(command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        UpdateCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        UpdateCustomerCommandHandler sut)
    {
        // Arrange

        customerApi.UpdateCustomer(command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApi.Received().UpdateCustomer(command.Dto);
    }
}
