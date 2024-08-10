using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Ordering.UnitTests.Application.Commands;

public class IdentifiedCommandHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task Handler_sends_command_when_order_no_exists(
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
    public async Task Handler_sends_no_command_when_order_already_exists(
        [Substitute, Frozen] IRequestManager requestManager,
        [Substitute, Frozen] IMediator mediator,
        CreateOrderIdentifiedCommandHandler sut,
        IdentifiedCommand<CreateOrderCommand, bool> command
    )
    {
        // Arrange

        requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(true));

        mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act

        var result = await sut.Handle(command, default);

        // Assert

        Assert.True (result);

        await mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }
}
