using eShop.WebApp.Services.OrderStatus;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebApp.Extensions;

internal static class ApiExtensions
{
    public static RouteGroupBuilder MapApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/orderStatus").HasApiVersion(1.0);

        api.MapPost("/{buyerIdentityGuid}", async (OrderStatusNotificationService orderStatusNotificationService, [FromRoute] Guid buyerIdentityGuid) =>
        {
            await orderStatusNotificationService.NotifyOrderStatusChangedAsync(buyerIdentityGuid);
        });
        
        return api;
    }
}
