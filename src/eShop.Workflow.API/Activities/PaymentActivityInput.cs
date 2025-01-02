namespace eShop.Workflow.API.Activities;

public record PaymentActivityInput(Guid OrderId, Guid BuyerId);
