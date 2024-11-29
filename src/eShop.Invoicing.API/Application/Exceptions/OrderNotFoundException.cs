namespace eShop.Invoicing.API.Application.Exceptions;

internal class OrderNotFoundException : Exception
{
    public OrderNotFoundException() : base("Order not found")
    {
    }
}
