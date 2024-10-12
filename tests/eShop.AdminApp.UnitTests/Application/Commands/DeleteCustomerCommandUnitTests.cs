using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;
using eShop.Customer.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class DeleteCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerDeleted(
        DeleteCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApi.Received().DeleteCustomer(command.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteCustomerCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        customerApi.DeleteCustomer(command.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApi.Received().DeleteCustomer(command.ObjectId);
    }
}
