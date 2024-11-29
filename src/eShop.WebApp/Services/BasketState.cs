using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.WebApp.Extensions;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.ServiceInvocation.BasketApiClient;
using eShop.Basket.Contracts.Grpc;

namespace eShop.WebApp.Services;

public class BasketState(
    IBasketApiClient basketApiClient,
    ICatalogApiClient catalogApiClient,
    IOrderingApiClient orderingApiClient,
    AuthenticationStateProvider authenticationStateProvider) : IBasketState
{
    private Task<BasketItem[]>? _cachedBasket;
    private readonly HashSet<BasketStateChangedSubscription> _changeSubscriptions = [];

    public Task DeleteBasketAsync()
        => basketApiClient.DeleteBasketAsync();

    public async Task<IReadOnlyCollection<BasketItem>> GetBasketItemsAsync()
        => (await this.GetUserAsync()).Identity?.IsAuthenticated == true
        ? await this.FetchBasketItemsAsync()
        : [];

    public IDisposable NotifyOnChange(EventCallback callback)
    {
        BasketStateChangedSubscription subscription = new(this, callback);
        this._changeSubscriptions.Add(subscription);
        return subscription;
    }

    public async Task AddAsync(CatalogItemViewModel item)
    {
        BasketItem[] items = await this.FetchBasketItemsAsync();
        bool found = false;
        for (int i = 0; i < items.Count(); i++)
        {
            BasketItem existing = items[i];
            if (existing.ProductId == item.ObjectId)
            {
                items[i].Quantity = existing.Quantity + 1;
                found = true;
                break;
            }
        }

        UpdateBasketRequest updateBasketRequest = new();
        foreach (BasketItem basketQuantity in items)
        {
            updateBasketRequest.Items.Add(new Basket.Contracts.Grpc.BasketItem
            {
                ProductId = basketQuantity.ProductId.ToString(),
                Quantity = basketQuantity.Quantity
            });
        }

        if (!found)
        {
            updateBasketRequest.Items.Add(new Basket.Contracts.Grpc.BasketItem
            {
                ProductId = item.ObjectId.ToString(),
                Quantity = 1
            });
        }

        this._cachedBasket = null;
        await basketApiClient.UpdateBasketAsync(updateBasketRequest);
        await this.NotifyChangeSubscribersAsync();
    }

    public async Task SetQuantityAsync(Guid productId, int quantity)
    {
        List<BasketItem> existingItems = [.. (await this.FetchBasketItemsAsync())];
        if (existingItems.FirstOrDefault(row => row.ProductId == productId) is { } row)
        {
            if (quantity > 0)
            {
                row.Quantity = quantity;
            }
            else
            {
                existingItems.Remove(row);
            }

            UpdateBasketRequest updateBasketRequest = new();
            foreach (BasketItem basketItem in existingItems)
            {
                updateBasketRequest.Items.Add(new Basket.Contracts.Grpc.BasketItem
                {
                    ProductId = basketItem.ProductId.ToString(),
                    Quantity = basketItem.Quantity
                });
            }

            this._cachedBasket = null;
            await basketApiClient.UpdateBasketAsync(updateBasketRequest);
            await this.NotifyChangeSubscribersAsync();
        }
    }

    public async Task CheckoutAsync(BasketCheckoutInfo checkoutInfo)
    {
        if (checkoutInfo.RequestId == default)
        {
            checkoutInfo.RequestId = Guid.NewGuid();
        }

        Guid buyerId = await authenticationStateProvider.GetBuyerIdAsync() ?? throw new InvalidOperationException("User does not have a buyer ID");
        string userName = await authenticationStateProvider.GetUserNameAsync() ?? throw new InvalidOperationException("User does not have a user name");
        string buyerName = await authenticationStateProvider.GetBuyerNameAsync() ?? throw new InvalidOperationException("User does not have a name");

        // Get details for the items in the basket
        IReadOnlyCollection<BasketItem> basketItems = await this.FetchBasketItemsAsync();
        IEnumerable<OrderItemDto> orderItems = basketItems.Select(_ => new OrderItemDto(
            _.ProductId, _.ProductName, _.UnitPrice, 0, _.Quantity, _.PictureUrl));

        // Call into Ordering.API to create the order using those details
        OrderDto request = new(
            UserId: buyerId,
            UserName: userName,
            BuyerName: buyerName,
            City: checkoutInfo.City!,
            Street: checkoutInfo.Street!,
            State: checkoutInfo.State!,
            Country: checkoutInfo.Country!,
            ZipCode: checkoutInfo.ZipCode!,
            CardNumber: "1111222233334444",
            CardHolderName: "TESTUSER",
            CardExpiration: DateTime.UtcNow.AddYears(1),
            CardSecurityNumber: "111",
            CardType: checkoutInfo.CardTypeId,
            Buyer: buyerId,
            Items: [.. orderItems]);
        await orderingApiClient.CreateOrder(checkoutInfo.RequestId, request);
        await this.DeleteBasketAsync();
    }

    private Task NotifyChangeSubscribersAsync()
        => Task.WhenAll(this._changeSubscriptions.Select(s => s.NotifyAsync()));

    private async Task<ClaimsPrincipal> GetUserAsync()
        => (await authenticationStateProvider.GetAuthenticationStateAsync()).User;

    private Task<BasketItem[]> FetchBasketItemsAsync()
    {
        return this._cachedBasket ??= FetchCoreAsync();

        async Task<BasketItem[]> FetchCoreAsync()
        {
            CustomerBasketResponse basket = await basketApiClient.GetBasketAsync();            
            if (basket.Items.Count == 0)
            {
                return [];
            }

            // Get details for the items in the basket
            List<BasketItem> basketItems = [];
            Guid[] productIds = basket.Items.Select(row => Guid.Parse(row.ProductId)).ToArray();
            Dictionary<Guid, CatalogItemViewModel> catalogItems = (await catalogApiClient.GetCatalogItems(productIds)).ToDictionary(k => k.ObjectId, v => v);
            foreach (Basket.Contracts.Grpc.BasketItem item in basket.Items)
            {
                Guid productId = Guid.Parse(item.ProductId);
                CatalogItemViewModel catalogItem = catalogItems[productId];
                BasketItem orderItem = new()
                {
                    Id = Guid.NewGuid().ToString(), // TODO: this value is meaningless, use ProductId instead.
                    ProductId = catalogItem.ObjectId,
                    ProductName = catalogItem.Name,
                    UnitPrice = catalogItem.Price,
                    Quantity = item.Quantity,
                    PictureUrl = catalogItem.PictureUrl
                };
                basketItems.Add(orderItem);
            }

            return basketItems.ToArray();
        }
    }

    private class BasketStateChangedSubscription(BasketState Owner, EventCallback Callback) : IDisposable
    {
        public Task NotifyAsync() => Callback.InvokeAsync();
        public void Dispose() => Owner._changeSubscriptions.Remove(this);
    }
}
