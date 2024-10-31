using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;
using Refit;

namespace eShop.Ordering.Contracts;

public interface IOrderingApi
{
    [Get("/api/orders/all?api-version=1.0")]
    Task<OrderDto[]> GetOrders();

    [Get("/api/orders/cardTypes?api-version=1.0")]
    Task<CardTypeDto[]> GetCardTypes();

    [Post("/api/orders?api-version=1.0")]
    Task CreateOrder([Header("x-requestid")] Guid requestId, CreateOrderDto request);
}
