using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.Http;
using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.API.Application.Queries;
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
        var handler = new ApiVersionHandler(new QueryStringApiVersionWriter(), new ApiVersion(1.0));

        this._webApplicationFactory = fixture;
        this._httpClient = this._webApplicationFactory.CreateDefaultClient(handler);
    }

    [Fact]
    public async Task GetAllStoredOrdersWorks()
    {
        // Act
        var response = await this._httpClient.GetAsync("api/orders");
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
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        var response = await this._httpClient.PutAsync("/api/orders/cancel", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CancelNonExistentOrderFails(
        CancelOrderCommand command)
    {
        // Act
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        var response = await this._httpClient.PutAsync("api/orders/cancel", content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task ShipWithEmptyGuidFails(
        ShipOrderCommand command)
    {
        // Act
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        var response = await this._httpClient.PutAsync("api/orders/ship", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task ShipNonExistentOrderFails(
        ShipOrderCommand command)
    {
        // Act
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        var response = await this._httpClient.PutAsync("api/orders/ship", content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetAllOrdersCardType()
    {
        // Act 1
        var response = await this._httpClient.GetAsync("api/orders/cardTypes");
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetStoredOrdersWithOrderId()
    {
        // Act
        var response = await this._httpClient.GetAsync("api/orders/1");
        var responseStatus = response.StatusCode;

        // Assert
        Assert.Equal("NotFound", responseStatus.ToString());
    }

    [Fact]
    public async Task AddNewEmptyOrder()
    {
        // Act
        var content = new StringContent(JsonSerializer.Serialize(new Order()), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.Empty.ToString() } }
        };
        var response = await this._httpClient.PostAsync("api/orders", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task AddNewOrder(
        CreateOrderDto order,
        OrderItemDto orderItem)
    {
        // Act

        IEnumerable<CardTypeDto> cardTypes = await this._httpClient.GetFromJsonAsync<IEnumerable<CardTypeDto>>("api/orders/cardTypes");
        CardTypeDto cardType = cardTypes.First(_ => _.Name == "Amex");

        var cardExpirationDate = DateTime.Now.AddYears(1);
        var orderRequest = new CreateOrderDto(order.UserId, order.UserName, null, null, null, null, null,
            order.CardNumber, order.CardHolderName, cardExpirationDate, order.CardSecurityNumber, cardType.ObjectId, null, [orderItem]);
        var content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        var response = await this._httpClient.PostAsync("api/orders", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task PostDraftOrder(
        OrderItemDto orderItem)
    {
        // Act
        
        var bodyContent = new { BuyerId = "1", Items = new List<OrderItemDto> { orderItem } };
        var content = new StringContent(JsonSerializer.Serialize(bodyContent), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        var response = await this._httpClient.PostAsync("api/orders/draft", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CreateOrderDraftSucceeds(
        CreateOrderDraftCommand command)
    {
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json")
        {
            Headers = { { "x-requestid", Guid.NewGuid().ToString() } }
        };
        var response = await this._httpClient.PostAsync("api/orders/draft", content);

        var s = await response.Content.ReadAsStringAsync();
        var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var options = jsonSerializerOptions;
        var responseData = JsonSerializer.Deserialize<OrderDraftDTO>(s, options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(command.Items.Count(), responseData.OrderItems.Count());
        Assert.Equal(command.Items.Sum(o => o.Units * o.UnitPrice), responseData.Total);
        AssertThatOrderItemsAreTheSameAsRequestPayloadItems(command, responseData);
    }

    private static void AssertThatOrderItemsAreTheSameAsRequestPayloadItems(CreateOrderDraftCommand payload, OrderDraftDTO responseData)
    {
        // check that OrderItems contain all product Ids from the payload
        var payloadItemsProductIds = payload.Items.Select(x => x.ProductId);
        var orderItemsProductIds = responseData.OrderItems.Select(x => x.ProductId);
        Assert.All(orderItemsProductIds, orderItemProdId => payloadItemsProductIds.Contains(orderItemProdId));
        // TODO: might need to add more asserts in here
    }
}
