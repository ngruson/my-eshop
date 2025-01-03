using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.SetAwaitingValidationOrderStatus;
using eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;
using eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;
using eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Commands;

public class IdentifiedCommandHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_create_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, Result<Guid>> command,
        Guid orderId
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<CreateOrderCommand>(), default)
            .Returns(Task.FromResult(Result.Success(orderId)));

        // Act

        Result<Guid> result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<CreateOrderCommand>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_cancel_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CancelOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CancelOrderCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<Result>>(), default)
            .Returns(Task.FromResult(Result.Success()));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<IRequest<Result>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_ship_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        ShipOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<ShipOrderCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<Result>>(), default)
            .Returns(Task.FromResult(Result.Success()));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<IRequest<Result>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_paid_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetPaidIdentifiedOrderStatusCommandHandler sut,
        IdentifiedCommand<SetPaidOrderStatusCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<Result>>(), default)
            .Returns(Task.FromResult(Result.Success()));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<IRequest<Result>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_awaiting_validation_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetAwaitingValidationIdentifiedOrderStatusCommandHandler sut,
        IdentifiedCommand<SetAwaitingValidationOrderStatusCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(command.Id)
            .Returns(Task.FromResult(false));

        mediator.Send(command.Command, default)
            .Returns(Result.Success());

        // Act
        Result result = await sut.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        await mediator.Received().Send(command.Command, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_stock_confirmed_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetStockConfirmedOrderStatusIdentifiedCommandHandler sut,
        IdentifiedCommand<SetStockConfirmedOrderStatusCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<SetStockConfirmedOrderStatusCommand>(), default)
            .Returns(Task.FromResult(Result.Success()));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert

        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<SetStockConfirmedOrderStatusCommand>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_stock_rejected_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetStockRejectedOrderStatusIdentifiedCommandHandler sut,
        IdentifiedCommand<SetStockRejectedOrderStatusCommand, Result> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<SetStockRejectedOrderStatusCommand>(), default)
            .Returns(Task.FromResult(Result.Success()));

        // Act

        Result result = await sut.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        await mediator.Received().Send(Arg.Any<SetStockRejectedOrderStatusCommand>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_no_command_when_order_already_exists(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, Result<Guid>> message
    )
    {
        // Arrange

        requestManager.ExistAsync(message.Id)
            .Returns(Task.FromResult(true));

        // Act

        Result<Guid> result = await sut.Handle(message, default);

        // Assert

        Assert.True(result.IsSuccess);

        await mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_when_exception_gets_thrown_return_false(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, Result<Guid>> message
    )
    {
        // Arrange

        requestManager.ExistAsync(message.Id)
            .Returns(Task.FromResult(false));

        mediator.Send(message.Command, default)
            .ThrowsAsync<Exception>();

        // Act

        Result<Guid> result = await sut.Handle(message, default);

        // Assert

        Assert.Null(result);

        await mediator.Received().Send(message.Command, default);
    }
}
