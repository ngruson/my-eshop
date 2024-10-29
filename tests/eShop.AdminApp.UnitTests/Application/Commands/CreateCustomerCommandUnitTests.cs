using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Customer.CreateCustomer;
using eShop.ServiceInvocation.CustomerService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class CreateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerService.Received().CreateCustomer(command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        customerService.CreateCustomer(command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerService.Received().CreateCustomer(command.Dto);
    }
}
