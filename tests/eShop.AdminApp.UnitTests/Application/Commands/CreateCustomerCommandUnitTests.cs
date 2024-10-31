using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Customer.CreateCustomer;
using eShop.ServiceInvocation.CustomerApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class CreateCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerApiClient customerApiClient,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApiClient.Received().CreateCustomer(command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateCustomerCommand command,
        [Substitute, Frozen] ICustomerApiClient customerApiClient,
        CreateCustomerCommandHandler sut)
    {
        // Arrange

        customerApiClient.CreateCustomer(command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApiClient.Received().CreateCustomer(command.Dto);
    }
}
