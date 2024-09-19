namespace eShop.Customer.API.Application.Exceptions;

internal class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException() : base("Customer not found")
    {
    }
}
