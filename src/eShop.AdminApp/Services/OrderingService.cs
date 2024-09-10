using eShop.Ordering.Contracts.GetOrders;

namespace eShop.AdminApp.Services;

public class OrderingService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrl = "/api/Orders/";

    public Task<OrderDto[]> GetOrders()
    {
        return httpClient.GetFromJsonAsync<OrderDto[]>($"{this.remoteServiceBaseUrl}all")!;
    }
}
