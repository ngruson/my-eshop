using CommunityToolkit.Mvvm.Messaging;
using eShop.ClientApp.Messages;
using eShop.ClientApp.Models.Basket;
using eShop.ClientApp.Models.Orders;
using eShop.ClientApp.Models.User;
using eShop.ClientApp.Services;
using eShop.ClientApp.Services.AppEnvironment;
using eShop.ClientApp.Services.Settings;
using eShop.ClientApp.ViewModels.Base;

namespace eShop.ClientApp.ViewModels;

public partial class CheckoutViewModel(
    BasketViewModel basketViewModel,
    IAppEnvironmentService appEnvironmentService, IDialogService dialogService, ISettingsService settingsService,
    INavigationService navigationService) : ViewModelBase(navigationService)
{
    private readonly IAppEnvironmentService _appEnvironmentService = appEnvironmentService;

    private readonly BasketViewModel _basketViewModel = basketViewModel;
    private readonly IDialogService _dialogService = dialogService;
    private readonly ISettingsService _settingsService = settingsService;

    [ObservableProperty] private Order? _order;

    [ObservableProperty] private Address? _shippingAddress;

    public override async Task InitializeAsync()
    {
        await this.IsBusyFor(
            async () =>
            {
                var basketItems = this._appEnvironmentService.BasketService.LocalBasketItems;

                var userInfo = await this._appEnvironmentService.IdentityService.GetUserInfoAsync();

                // Create Shipping Address
                this.ShippingAddress = new Address
                {
                    Id = !string.IsNullOrEmpty(userInfo?.UserId) ? new Guid(userInfo.UserId) : Guid.NewGuid(),
                    Street = userInfo?.Street,
                    ZipCode = userInfo?.ZipCode,
                    State = userInfo?.State,
                    Country = userInfo?.Country,
                    City = userInfo?.Address
                };

                // Create Payment Info
                var paymentInfo = new PaymentInfo
                {
                    CardNumber = userInfo?.CardNumber,
                    CardHolderName = userInfo?.CardHolder,
                    CardType = new CardType {Id = 3, Name = "MasterCard"},
                    SecurityNumber = userInfo?.CardSecurityNumber
                };

                var orderItems = CreateOrderItems(basketItems);

                // Create new Order
                this.Order = new Order
                {
                    //TODO: Get a better order number generator
                    OrderNumber = (int)DateTimeOffset.Now.TimeOfDay.TotalMilliseconds,
                    UserId = userInfo.UserId,
                    UserName = userInfo.PreferredUsername,
                    OrderItems = orderItems,
                    OrderStatus = "Submitted",
                    OrderDate = DateTime.Now,
                    CardHolderName = paymentInfo.CardHolderName,
                    CardNumber = paymentInfo.CardNumber,
                    CardSecurityNumber = paymentInfo.SecurityNumber,
                    CardExpiration = DateTime.UtcNow.AddYears(5),
                    CardTypeId = paymentInfo.CardType.Id,
                    ShippingState = ShippingAddress.State,
                    ShippingCountry = ShippingAddress.Country,
                    ShippingStreet = ShippingAddress.Street,
                    ShippingCity = ShippingAddress.City,
                    ShippingZipCode = ShippingAddress.ZipCode,
                    Total = CalculateTotal(orderItems)
                };

                if (_settingsService.UseMocks)
                {
                    // Get number of orders
                    var orders = await _appEnvironmentService.OrderService.GetOrdersAsync();

                    // Create the OrderNumber
                    Order.OrderNumber = orders.Count() + 1;
                    OnPropertyChanged(nameof(Order));
                }
            });
    }

    [RelayCommand]
    private async Task CheckoutAsync()
    {
        try
        {
            var basket = _appEnvironmentService.OrderService.MapOrderToBasket(Order);
            basket.RequestId = Guid.NewGuid();

            await _appEnvironmentService.OrderService.CreateOrderAsync(Order);

            // Clean Basket
            await _appEnvironmentService.BasketService.ClearBasketAsync();

            // Reset Basket badge
            await _basketViewModel.ClearBasketItems();
            
            WeakReferenceMessenger.Default
                .Send(new ProductCountChangedMessage(0));

            // Navigate to Orders
            await NavigationService.NavigateToAsync("//Main/Catalog");

            // Show Dialog
            await _dialogService.ShowAlertAsync("Order sent successfully!", "Checkout", "Ok");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await _dialogService.ShowAlertAsync("An error ocurred. Please, try again.", "Oops!", "Ok");
        }
    }

    private static List<OrderItem> CreateOrderItems(IEnumerable<BasketItem> basketItems)
    {
        var orderItems = new List<OrderItem>();

        foreach (var basketItem in basketItems)
        {
            if (!string.IsNullOrEmpty(basketItem.ProductName))
            {
                orderItems.Add(new OrderItem
                {
                    OrderId = null,
                    ProductId = basketItem.ProductId,
                    ProductName = basketItem.ProductName,
                    PictureUrl = basketItem.PictureUrl,
                    Quantity = basketItem.Quantity,
                    UnitPrice = basketItem.UnitPrice
                });
            }
        }

        return orderItems;
    }

    private static decimal CalculateTotal(List<OrderItem> orderItems)
    {
        decimal total = 0;

        foreach (var orderItem in orderItems)
        {
            total += orderItem.Quantity * orderItem.UnitPrice;
        }

        return total;
    }
}
