using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.Http;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;
using eShop.Customer.Domain.AggregatesModel.CustomerAggregate;
using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Customer.FunctionalTests;

public sealed class CustomerApiTests : IClassFixture<CustomerApiFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public CustomerApiTests(CustomerApiFixture fixture)
    {
        ApiVersionHandler handler = new(new QueryStringApiVersionWriter(), new ApiVersion(1.0));

        this._webApplicationFactory = fixture;
        this._httpClient = this._webApplicationFactory.CreateDefaultClient(handler);
    }

    [Fact]
    public async Task GetCustomers_ReturnCustomersGivenCustomersExist()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/all");

        // Assert

        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        List<CustomerDto> result = JsonSerializer.Deserialize<List<CustomerDto>>(body, this._jsonSerializerOptions);

        Assert.Equal(10, result.Count);
    }

    [Fact]
    public async Task GetCustomerByObjectId_ReturnOkGivenCustomerExist()
    {
        // Arrange

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/Bob%20Smith");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        CustomerDto result = JsonSerializer.Deserialize<CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        response = await this._httpClient.GetAsync($"/api/customers/{result.ObjectId}");

        // Assert

        response.EnsureSuccessStatusCode();
        body = await response.Content.ReadAsStringAsync();
        result = JsonSerializer.Deserialize<CustomerDto>(body, this._jsonSerializerOptions);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCustomerByObjectId_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Arrange

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync($"/api/customers/{Guid.NewGuid()}");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomerByName_ReturnOkGivenCustomerExist()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/Bob%20Smith");

        // Assert

        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        CustomerDto result = JsonSerializer.Deserialize<CustomerDto>(body, this._jsonSerializerOptions);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCustomerByName_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/John%20Doe");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory, AutoNSubstituteData]
    public async Task CreateCustomer_ReturnOkGivenCustomerCreated(
        CreateCustomerDto dto)
    {
        // Act

        HttpResponseMessage response = await this._httpClient.PostAsync(
            "/api/customers",
            new StringContent(
                JsonSerializer.Serialize(dto with { CardType = CardType.Amex.Name },
                    this._jsonSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"
            ));

        // Assert

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCustomer_ReturnOkGivenCustomerUpdated()
    {
        // Arrange

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/Immanuel%20Gooding");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCustomer.CustomerDto customer =
            JsonSerializer.Deserialize<Contracts.GetCustomer.CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        UpdateCustomerDto updateCustomerDto = new(            
            customer.UserName,
            customer.FirstName,
            customer.LastName,
            customer.Street,
            customer.City,
            customer.State,
            customer.Country,
            customer.ZipCode);

        response = await this._httpClient.PutAsync(
            $"/api/customers/{customer.ObjectId}/generalInfo",
            new StringContent(
                JsonSerializer.Serialize(updateCustomerDto,
                    this._jsonSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"));

        // Assert

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCustomer_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Arrange

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/Sunny%20Swinnerton");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCustomer.CustomerDto customer =
            JsonSerializer.Deserialize<Contracts.GetCustomer.CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        UpdateCustomerDto updateCustomerDto = new(
            customer.UserName,
            "Sunny2",
            "Swinnerton2",
            customer.Street,
            customer.City,
            customer.State,
            customer.Country,
            customer.ZipCode);

        response = await this._httpClient.PutAsync(
            $"/api/customers/{Guid.NewGuid()}/generalInfo",
            new StringContent(
                JsonSerializer.Serialize(updateCustomerDto,
                    this._jsonSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"
            ));

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCustomer_ReturnOkGivenCustomerDeleted()
    {
        // Arrange

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers/name/Alice%20Smith");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        CustomerDto result = JsonSerializer.Deserialize<CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        response = await this._httpClient.DeleteAsync(
            $"/api/customers/{result.ObjectId}");

        // Assert

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteCustomer_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Arrange

        // Act

        HttpResponseMessage response = await this._httpClient.DeleteAsync(
            $"/api/customers/{Guid.NewGuid()}");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
