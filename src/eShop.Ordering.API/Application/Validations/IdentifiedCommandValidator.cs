using Ardalis.Result;
using eShop.Ordering.API.Application.Commands.CreateOrder;

namespace eShop.Ordering.API.Application.Validations;

public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand, Result>>
{
    public IdentifiedCommandValidator()
    {
        this.RuleFor(command => command.Id).NotEmpty();
    }
}
