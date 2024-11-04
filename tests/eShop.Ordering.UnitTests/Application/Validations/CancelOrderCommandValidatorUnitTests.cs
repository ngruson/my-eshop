using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Validations;
using FluentValidation.TestHelper;

namespace Ordering.UnitTests.Application.Validations;
public class CancelOrderCommandValidatorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void valid(
        CancelOrderCommandValidator sut,
        CancelOrderCommand command
    )
    {
        // Arrange

        // Act

        TestValidationResult<CancelOrderCommand> result = sut.TestValidate(command);

        //Assert

        Assert.True(result.IsValid);
    }

    [Theory, AutoNSubstituteData]
    internal void empty(
        CancelOrderCommandValidator sut
    )
    {
        // Arrange

        CancelOrderCommand command = new(0);

        // Act

        TestValidationResult<CancelOrderCommand> result = sut.TestValidate(command);

        //Assert

        Assert.False(result.IsValid);
    }
}
