using eShop.ClientApp.Services.Basket;
using eShop.ClientApp.Services.Catalog;
using eShop.ClientApp.Services.Identity;
using eShop.ClientApp.Services.Order;

namespace eShop.ClientApp.Services.AppEnvironment;

public class AppEnvironmentService(
    IBasketService mockBasketService, IBasketService basketService,
    ICatalogService mockCatalogService, ICatalogService catalogService,
    IOrderService mockOrderService, IOrderService orderService,
    IIdentityService mockIdentityService, IIdentityService identityService) : IAppEnvironmentService
{
    private readonly IBasketService _basketService = basketService;
    private readonly ICatalogService _catalogService = catalogService;
    private readonly IIdentityService _identityService = identityService;
    private readonly IBasketService _mockBasketService = mockBasketService;

    private readonly ICatalogService _mockCatalogService = mockCatalogService;

    private readonly IIdentityService _mockIdentityService = mockIdentityService;

    private readonly IOrderService _mockOrderService = mockOrderService;
    private readonly IOrderService _orderService = orderService;

    public IBasketService? BasketService { get; private set; }

    public ICatalogService? CatalogService { get; private set; }

    public IOrderService? OrderService { get; private set; }

    public IIdentityService? IdentityService { get; private set; }

    public void UpdateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            this.BasketService = this._mockBasketService;
            this.CatalogService = this._mockCatalogService;
            this.OrderService = this._mockOrderService;
            this.IdentityService = this._mockIdentityService;
        }
        else
        {
            this.BasketService = this._basketService;
            this.CatalogService = this._catalogService;
            this.OrderService = this._orderService;
            this.IdentityService = this._identityService;
        }
    }
}
