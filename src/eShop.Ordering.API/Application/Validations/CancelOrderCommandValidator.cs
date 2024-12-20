using eShop.Ordering.API.Application.Commands.CancelOrder;

namespace eShop.Ordering.API.Application.Validations;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        this.RuleFor(order => order.ObjectId).NotEmpty().WithMessage("No orderId found");
    }
}
