using System.Diagnostics.CodeAnalysis;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.Model;
using static eShop.Basket.Contracts.Grpc.Basket;
using eShop.Basket.Contracts.Grpc;
using Ardalis.Result;

namespace eShop.Basket.API.Grpc;

public class BasketService(
    IBasketRepository repository,
    ILogger<BasketService> logger) : BasketBase
{
    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        string? userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            return new();
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Begin GetBasketById call from method {Method} for basket id {Id}", context.Method, userId);
        }

        Result<CustomerBasket> result = await repository.GetBasketAsync(userId);

        if (result.IsSuccess)
        {
            return MapToCustomerBasketResponse(result.Value);
        }

        return new();
    }

    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        string? userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Begin UpdateBasket call from method {Method} for basket id {Id}", context.Method, userId);
        }

        CustomerBasket customerBasket = MapToCustomerBasket(userId, request);
        Result<CustomerBasket> result = await repository.UpdateBasketAsync(customerBasket);
        if (result.IsNotFound())
        {
            ThrowBasketDoesNotExist(userId);
        }

        return MapToCustomerBasketResponse(result.Value);
    }

    public override async Task<DeleteBasketResponse> DeleteBasket(DeleteBasketRequest request, ServerCallContext context)
    {
        string? userId = context.GetUserIdentity();
        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        await repository.DeleteBasketAsync(userId);
        return new();
    }

    [DoesNotReturn]
    private static void ThrowNotAuthenticated() => throw new RpcException(new Status(StatusCode.Unauthenticated, "The caller is not authenticated."));

    [DoesNotReturn]
    private static void ThrowBasketDoesNotExist(string userId) => throw new RpcException(new Status(StatusCode.NotFound, $"Basket with buyer id {userId} does not exist"));

    private static CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket? customerBasket)
    {
        CustomerBasketResponse response = new();

        if (customerBasket is not null)
        {
            foreach (Model.BasketItem item in customerBasket.Items)
            {
                response.Items.Add(new Contracts.Grpc.BasketItem()
                {
                    ProductId = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                });
            }
        }        

        return response;
    }

    private static CustomerBasket MapToCustomerBasket(string userId, UpdateBasketRequest customerBasketRequest)
    {
        CustomerBasket response = new()
        {
            BuyerId = userId
        };

        foreach (Contracts.Grpc.BasketItem item in customerBasketRequest.Items)
        {
            response.Items.Add(new()
            {
                ProductId = Guid.Parse(item.ProductId),
                Quantity = item.Quantity,
            });
        }

        return response;
    }
}
