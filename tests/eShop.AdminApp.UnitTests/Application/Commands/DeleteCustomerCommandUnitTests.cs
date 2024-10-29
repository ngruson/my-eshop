using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;
using eShop.ServiceInvocation.CustomerService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class DeleteCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerDeleted(
        DeleteCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerService.Received().DeleteCustomer(command.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        DeleteCustomerCommandHandler sut)
    {
        // Arrange

        customerService.DeleteCustomer(command.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerService.Received().DeleteCustomer(command.ObjectId);
    }
}
