using eShop.Ordering.API.Application.Commands.CreateOrder;

namespace eShop.Ordering.API.Application.Validations;

public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand, bool>>
{
    public IdentifiedCommandValidator()
    {
        this.RuleFor(command => command.Id).NotEmpty();
    }
}
