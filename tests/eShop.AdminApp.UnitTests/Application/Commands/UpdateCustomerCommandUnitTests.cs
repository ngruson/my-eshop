using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using eShop.AdminApp.Application.Commands.Customer.UpdateCustomer;
using eShop.ServiceInvocation.CustomerService;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class UpdateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerUpdated(
        UpdateCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        UpdateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerService.Received().UpdateCustomer(command.ObjectId, command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        UpdateCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        UpdateCustomerCommandHandler sut)
    {
        // Arrange

        customerService.UpdateCustomer(command.ObjectId, command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerService.Received().UpdateCustomer(command.ObjectId, command.Dto);
    }
}
