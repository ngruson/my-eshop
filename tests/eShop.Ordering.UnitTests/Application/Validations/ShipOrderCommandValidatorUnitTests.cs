using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.API.Application.Validations;
using FluentValidation.TestHelper;

namespace Ordering.UnitTests.Application.Validations;
public class ShipOrderCommandValidatorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void valid(
        ShipOrderCommandValidator sut,
        ShipOrderCommand command
    )
    {
        // Arrange

        // Act

        TestValidationResult<ShipOrderCommand> result = sut.TestValidate(command);

        //Assert

        Assert.True(result.IsValid);
    }

    [Theory, AutoNSubstituteData]
    internal void order_number_empty_invalid(
        ShipOrderCommandValidator sut,
        ShipOrderCommand command
    )
    {
        // Arrange

        // Act

        TestValidationResult<ShipOrderCommand> result = sut.TestValidate(command with { OrderNumber = 0 });

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.OrderNumber), result.Errors.Select(_ => _.PropertyName));
    }
}
