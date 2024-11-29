using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Invoicing.API.Application.Commands.CreateInvoice;
using eShop.Invoicing.API.Application.Storage;
using eShop.Ordering.Contracts.GetOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Invoicing.UnitTests;

public class CreateInvoiceUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_invoice_was_created(
        [Substitute, Frozen] IOrderingApiClient apiClient,
        [Substitute, Frozen] IFileStorage fileStorage,
        CreateInvoiceCommandHandler sut,
        CreateInvoiceCommand command,
        OrderDto dto)
    {
        // Arrange

        apiClient.GetOrder(command.OrderId)
            .Returns(dto);

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        await fileStorage.Received().UploadFile(Arg.Any<string>(), Arg.Any<byte[]>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_not_found_when_order_does_not_exist(
        [Substitute, Frozen] IFileStorage fileStorage,
        CreateInvoiceCommandHandler sut,
        CreateInvoiceCommand command)
    {
        // Arrange

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsNotFound());
        await fileStorage.DidNotReceive().UploadFile(Arg.Any<string>(), Arg.Any<byte[]>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_was_thrown(
        [Substitute, Frozen] IOrderingApiClient apiClient,
        [Substitute, Frozen] IFileStorage fileStorage,
        CreateInvoiceCommandHandler sut,
        CreateInvoiceCommand command)
    {
        // Arrange

        apiClient.GetOrder(command.OrderId)
            .ThrowsAsync<Exception>();

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsError());
        await fileStorage.DidNotReceive().UploadFile(Arg.Any<string>(), Arg.Any<byte[]>());
    }
}
