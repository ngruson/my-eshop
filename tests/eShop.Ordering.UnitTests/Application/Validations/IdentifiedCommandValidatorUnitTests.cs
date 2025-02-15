using Ardalis.Result;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Validations;
using FluentValidation.TestHelper;

namespace eShop.Ordering.UnitTests.Application.Validations;
public class IdentifiedCommandValidatorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void valid(
        IdentifiedCommandValidator sut,
        IdentifiedCommand<CreateOrderCommand, Result<Guid>> command
    )
    {
        // Arrange

        // Act

        TestValidationResult<IdentifiedCommand<CreateOrderCommand, Result<Guid>>> result = sut.TestValidate(command);

        //Assert

        Assert.True(result.IsValid);
    }

    [Theory, AutoNSubstituteData]
    internal void id_empty_invalid(
        IdentifiedCommandValidator sut,
        CreateOrderCommand command
    )
    {
        // Arrange

        IdentifiedCommand<CreateOrderCommand, Result<Guid>> request = new(command, Guid.Empty);

        // Act

        TestValidationResult<IdentifiedCommand<CreateOrderCommand, Result<Guid>>> result = sut.TestValidate(request);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(request.Id), result.Errors.Select(_ => _.PropertyName));
    }
}
