namespace eShop.Ordering.API.Application.Validations;

public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        this.RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");
    }
}
