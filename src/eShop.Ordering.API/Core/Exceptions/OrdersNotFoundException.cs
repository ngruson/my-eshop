namespace eShop.Ordering.API.Core.Exceptions;

[Serializable]
internal class OrdersNotFoundException : Exception
{
    public OrdersNotFoundException() : base("Orders not found")
    {
    }
}
