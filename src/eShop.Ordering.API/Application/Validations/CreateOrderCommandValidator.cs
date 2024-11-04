using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.Contracts.CreateOrder;

namespace eShop.Ordering.API.Application.Validations;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        this.RuleFor(command => command.City).NotEmpty();
        this.RuleFor(command => command.Street).NotEmpty();
        this.RuleFor(command => command.State).NotEmpty();
        this.RuleFor(command => command.Country).NotEmpty();
        this.RuleFor(command => command.ZipCode).NotEmpty();
        this.RuleFor(command => command.CardNumber).NotEmpty().Length(12, 19);
        this.RuleFor(command => command.CardHolderName).NotEmpty();
        this.RuleFor(command => command.CardExpiration).NotEmpty().Must(this.BeValidExpirationDate).WithMessage("Please specify a valid card expiration date");
        this.RuleFor(command => command.CardSecurityNumber).NotEmpty().Length(3);
        this.RuleFor(command => command.CardType).NotEmpty();
        this.RuleFor(command => command.OrderItems).Must(this.ContainOrderItems).WithMessage("No order items found");
    }

    private bool BeValidExpirationDate(DateTime dateTime)
    {
        return dateTime >= DateTime.UtcNow;
    }

    private bool ContainOrderItems(IEnumerable<OrderItemDto> orderItems)
    {
        return orderItems.Any();
    }
}
