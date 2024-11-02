using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.WebApp.Extensions;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.OrderingApiClient;

namespace eShop.WebApp.Services;

public class BasketState(
    BasketService basketService,
    ICatalogApiClient catalogApiClient,
    IOrderingApiClient orderingApiClient,
    AuthenticationStateProvider authenticationStateProvider) : IBasketState
{
    private Task<IReadOnlyCollection<BasketItem>>? _cachedBasket;
    private readonly HashSet<BasketStateChangedSubscription> _changeSubscriptions = [];

    public Task DeleteBasketAsync()
        => basketService.DeleteBasketAsync();

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
        List<BasketQuantity> items = (await this.FetchBasketItemsAsync())
            .Select(i => new BasketQuantity(i.ProductId, i.Quantity)).ToList();
        bool found = false;
        for (int i = 0; i < items.Count; i++)
        {
            BasketQuantity existing = items[i];
            if (existing.ProductId == item.ObjectId)
            {
                items[i] = existing with { Quantity = existing.Quantity + 1 };
                found = true;
                break;
            }
        }

        if (!found)
        {
            items.Add(new BasketQuantity(item.ObjectId, 1));
        }

        this._cachedBasket = null;
        await basketService.UpdateBasketAsync(items);
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

            this._cachedBasket = null;
            await basketService.UpdateBasketAsync(existingItems.Select(i => new BasketQuantity(i.ProductId, i.Quantity)).ToList());
            await this.NotifyChangeSubscribersAsync();
        }
    }

    public async Task CheckoutAsync(BasketCheckoutInfo checkoutInfo)
    {
        if (checkoutInfo.RequestId == default)
        {
            checkoutInfo.RequestId = Guid.NewGuid();
        }

        string buyerId = await authenticationStateProvider.GetBuyerIdAsync() ?? throw new InvalidOperationException("User does not have a buyer ID");
        string userName = await authenticationStateProvider.GetUserNameAsync() ?? throw new InvalidOperationException("User does not have a user name");

        // Get details for the items in the basket
        IReadOnlyCollection<BasketItem> basketItems = await this.FetchBasketItemsAsync();
        IEnumerable<OrderItemDto> orderItems = basketItems.Select(_ => new OrderItemDto(
            _.ProductId, _.ProductName, _.UnitPrice, 0, _.Quantity, _.PictureUrl));

        // Call into Ordering.API to create the order using those details
        CreateOrderDto request = new(
            UserId: buyerId,
            UserName: userName,
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

    private Task<IReadOnlyCollection<BasketItem>> FetchBasketItemsAsync()
    {
        return this._cachedBasket ??= FetchCoreAsync();

        async Task<IReadOnlyCollection<BasketItem>> FetchCoreAsync()
        {
            IReadOnlyCollection<BasketQuantity> quantities = await basketService.GetBasketAsync();
            if (quantities.Count == 0)
            {
                return [];
            }

            // Get details for the items in the basket
            List<BasketItem> basketItems = [];
            Guid[] productIds = quantities.Select(row => row.ProductId).ToArray();
            Dictionary<Guid, CatalogItemViewModel> catalogItems = (await catalogApiClient.GetCatalogItems(productIds)).ToDictionary(k => k.ObjectId, v => v);
            foreach (BasketQuantity item in quantities)
            {
                CatalogItemViewModel catalogItem = catalogItems[item.ProductId];
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

            return basketItems;
        }
    }

    private class BasketStateChangedSubscription(BasketState Owner, EventCallback Callback) : IDisposable
    {
        public Task NotifyAsync() => Callback.InvokeAsync();
        public void Dispose() => Owner._changeSubscriptions.Remove(this);
    }
}
