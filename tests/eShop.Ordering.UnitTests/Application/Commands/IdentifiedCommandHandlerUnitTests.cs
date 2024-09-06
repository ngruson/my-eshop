using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute.ExceptionExtensions;

namespace Ordering.UnitTests.Application.Commands;

public class IdentifiedCommandHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_create_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_cancel_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CancelOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CancelOrderCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_ship_order_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        ShipOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<ShipOrderCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_paid_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetPaidIdentifiedOrderStatusCommandHandler sut,
        IdentifiedCommand<SetPaidOrderStatusCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_awaiting_validation_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetAwaitingValidationIdentifiedOrderStatusCommandHandler sut,
        IdentifiedCommand<SetAwaitingValidationOrderStatusCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_stock_confirmed_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetStockConfirmedOrderStatusIdentifiedCommandHandler sut,
        IdentifiedCommand<SetStockConfirmedOrderStatusCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_stock_rejected_command_when_request_does_not_exist(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        SetStockRejectedOrderStatusIdentifiedCommandHandler sut,
        IdentifiedCommand<SetStockRejectedOrderStatusCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        Assert.True(result);
        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_no_command_when_order_already_exists(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, bool> message
    )
    {
        // Arrange

        requestManager.ExistAsync(message.Id)
            .Returns(Task.FromResult(true));

        // Act

        var result = await sut.Handle(message, default);

        // Assert

        Assert.True (result);

        await mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task Handler_when_exception_gets_thrown_return_false(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, bool> message
    )
    {
        // Arrange

        requestManager.ExistAsync(message.Id)
            .Returns(Task.FromResult(false));

        mediator.Send(message.Command, default)
            .ThrowsAsync<Exception>();

        // Act

        var result = await sut.Handle(message, default);

        // Assert

        Assert.False(result);

        await mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }
}
