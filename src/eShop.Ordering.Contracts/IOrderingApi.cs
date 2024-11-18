using Refit;

namespace eShop.Ordering.Contracts;

public interface IOrderingApi
{
    [Get("/api/orders/all?api-version=1.0")]
    Task<GetOrders.OrderDto[]> GetOrders();

    [Get("/api/orders/{objectId}?api-version=1.0")]
    Task<GetOrder.OrderDto> GetOrder(Guid objectId);

    [Get("/api/orders/cardTypes?api-version=1.0")]
    Task<GetCardTypes.CardTypeDto[]> GetCardTypes();

    [Post("/api/orders?api-version=1.0")]
    Task CreateOrder([Header("x-requestid")] Guid requestId, CreateOrder.OrderDto request);

    [Put("/api/orders/{objectId}?api-version=1.0")]
    Task UpdateOrder(Guid objectId, UpdateOrder.OrderDto request);

    [Delete("/api/orders/{objectId}?api-version=1.0")]
    Task DeleteOrder(Guid objectId);
}
