namespace eShop.Customer.API.Application.Exceptions;

internal class CustomersNotFoundException : Exception
{
    public CustomersNotFoundException() : base("Customers not found")
    {
    }
}
