namespace eShop.Ordering.API.Application.Validations;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(ILogger<CreateOrderCommandValidator> logger)
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
        this.RuleFor(command => command.CardTypeId).NotEmpty();
        this.RuleFor(command => command.OrderItems).Must(ContainOrderItems).WithMessage("No order items found");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("INSTANCE CREATED - {ClassName}", this.GetType().Name);
        }
    }

    private bool BeValidExpirationDate(DateTime dateTime)
    {
        return dateTime >= DateTime.UtcNow;
    }

    private bool ContainOrderItems(IEnumerable<OrderItemDTO> orderItems)
    {
        return orderItems.Any();
    }
}