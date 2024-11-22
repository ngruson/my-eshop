using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.Http;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Ordering.FunctionalTests;

public sealed class OrderingApiTests : IClassFixture<OrderingApiFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;

    public OrderingApiTests(OrderingApiFixture fixture)
    {
        ApiVersionHandler handler = new(new QueryStringApiVersionWriter(), new ApiVersion(1.0));

        this._webApplicationFactory = fixture;
        this._httpClient = this._webApplicationFactory.CreateDefaultClient(handler);
    }

    [Fact]
    public async Task GetAllStoredOrdersWorks()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("api/orders");
        await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CancelWithEmptyGuidFails(
        CancelOrderCommand command)
    {
        // Act

        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PutAsync("/api/orders/cancel", content);

        // Assert

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CancelNonExistentOrderFails(
        CancelOrderCommand command)
    {
        // Act

        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PutAsync("api/orders/cancel", content);

        // Assert

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task ShipWithEmptyGuidFails(
        ShipOrderCommand command)
    {
        // Act

        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PutAsync("api/orders/ship", content);

        // Assert

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task ShipNonExistentOrderFails(
        ShipOrderCommand command)
    {
        // Act

        StringContent content = new(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PutAsync("api/orders/ship", content);

        // Assert

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllOrdersCardType()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("api/orders/cardTypes");
        response.EnsureSuccessStatusCode();

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task GetStoredOrdersWithOrderId(Contracts.GetOrder.OrderDto order)
    {
        // Arrange

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync($"api/orders/{order.ObjectId}");
        HttpStatusCode responseStatus = response.StatusCode;

        // Assert

        Assert.Equal("NotFound", responseStatus.ToString());
    }

    [Theory, AutoNSubstituteData]
    public async Task AddNewEmptyOrder(OrderDto order)
    {
        // Act

        StringContent content = new(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PostAsync("api/orders", content);

        // Assert

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task AddNewOrder(
        OrderDto order,
        OrderItemDto orderItem)
    {
        // Act

        IEnumerable<CardTypeDto> cardTypes = await this._httpClient.GetFromJsonAsync<IEnumerable<CardTypeDto>>("api/orders/cardTypes");
        CardTypeDto cardType = cardTypes.First(_ => _.Name == "Amex");

        string cardNumber = order.CardNumber[..12];
        string cardSecurityNumber = order.CardSecurityNumber[..3];
        DateTime cardExpirationDate = DateTime.Now.AddYears(1);
        OrderDto orderRequest = new(order.UserId, order.UserName, order.City, order.Street, order.State, order.Country, order.ZipCode,
            cardNumber, order.CardHolderName, cardExpirationDate, cardSecurityNumber, cardType.ObjectId, order.UserId,
            [orderItem with {  Discount = 0 }]);
        StringContent content = new(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PostAsync("api/orders", content);

        // Assert
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CreateOrderDraftSucceeds(
    CreateOrderDraftCommand command)
    {
        OrderItemDto[] orderItems = command.Items
            .Select(x => new OrderItemDto(x.ProductId, x.ProductName, x.UnitPrice, 0, x.Units, x.PictureUrl))
            .ToArray();

        StringContent content = new(JsonSerializer.Serialize(command with {  Items = orderItems }), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        HttpResponseMessage response = await this._httpClient.PostAsync("api/orders/draft", content);

        string s = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        JsonSerializerOptions options = jsonSerializerOptions;
        OrderDraftDTO responseData = JsonSerializer.Deserialize<OrderDraftDTO>(s, options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(command.Items.Length, responseData.OrderItems.Count());
        Assert.Equal(command.Items.Sum(o => o.Units * o.UnitPrice), responseData.Total);
        AssertThatOrderItemsAreTheSameAsRequestPayloadItems(command, responseData);
    }

    private static void AssertThatOrderItemsAreTheSameAsRequestPayloadItems(CreateOrderDraftCommand payload, OrderDraftDTO responseData)
    {
        // check that OrderItems contain all product Ids from the payload

        IEnumerable<Guid> payloadItemsProductIds = payload.Items.Select(x => x.ProductId);
        IEnumerable<Guid> orderItemsProductIds = responseData.OrderItems.Select(x => x.ProductId);

        foreach (Guid orderItemProdId in orderItemsProductIds)
        {
            Assert.Contains(orderItemProdId, payloadItemsProductIds);
        }
    }
}
