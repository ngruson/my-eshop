using Refit;

namespace eShop.Ordering.Contracts;

public interface IOrderingApi
{
    [Put("/api/orders/cancel/{objectId}?api-version=1.0")]
    Task Cancel(Guid objectId);

    [Post("/api/orders/confirmGracePeriod/{objectId}?api-version=1.0")]
    Task ConfirmGracePeriod(Guid objectId);

    [Post("/api/orders/confirmStock/{objectId}?api-version=1.0")]
    Task ConfirmStock(Guid objectId);

    [Post("/api/orders/paid/{objectId}?api-version=1.0")]
    Task Paid(Guid objectId);

    [Post("/api/orders/rejectStock/{objectId}?api-version=1.0")]
    Task RejectStock(Guid objectId, Guid[] orderStockItems);

    [Post("/api/orders?api-version=1.0")]
    Task<Guid> CreateOrder([Header("x-requestid")] Guid requestId, CreateOrder.OrderDto request);

    [Get("/api/orders/cardTypes?api-version=1.0")]
    Task<GetCardTypes.CardTypeDto[]> GetCardTypes();

    [Get("/api/orders/all?api-version=1.0")]
    Task<GetOrders.OrderDto[]> GetOrders();

    [Get("/api/orders/{objectId}?api-version=1.0")]
    Task<GetOrder.OrderDto> GetOrder(Guid objectId);

    [Put("/api/orders/{objectId}?api-version=1.0")]
    Task UpdateOrder(Guid objectId, UpdateOrder.OrderDto request);

    [Delete("/api/orders/{objectId}?api-version=1.0")]
    Task DeleteOrder(Guid objectId);
}
