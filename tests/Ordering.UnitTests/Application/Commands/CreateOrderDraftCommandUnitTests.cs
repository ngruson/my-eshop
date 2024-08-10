namespace Ordering.UnitTests.Application.Commands;
public class CreateOrderDraftCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task CreateDraftOrder(
        CreateOrderDraftCommandHandler sut,
        CreateOrderDraftCommand command)
    {
        // Arrange

        //Act

        var result = await sut.Handle(command, default);

        //Assert

        Assert.NotNull(result);
    }
}
