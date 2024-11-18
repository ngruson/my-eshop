using eShop.Ordering.API.Application.Commands.ShipOrder;

namespace eShop.Ordering.API.Application.Validations;

public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        this.RuleFor(order => order.ObjectId).NotEmpty().WithMessage("No orderId found");
    }
}
