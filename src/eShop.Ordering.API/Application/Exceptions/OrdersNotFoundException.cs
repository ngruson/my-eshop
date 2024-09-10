namespace eShop.Ordering.API.Application.Exceptions;

[Serializable]
internal class OrdersNotFoundException : Exception
{
    public OrdersNotFoundException() : base("Orders not found")
    {
    }
}
