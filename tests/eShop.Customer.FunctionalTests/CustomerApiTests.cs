using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.Http;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Customer.Contracts.UpdateCustomer;
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
    public async Task GetCustomer_ReturnOkGivenCustomerExist()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers?firstName=Bob&lastName=Smith");

        // Assert

        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        CustomerDto result = JsonSerializer.Deserialize<CustomerDto>(body, this._jsonSerializerOptions);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCustomer_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers?firstName=Bob2&lastName=Smith2");

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
                JsonSerializer.Serialize(dto,
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

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers?firstName=Bob&lastName=Smith");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCustomer.CustomerDto customer =
            JsonSerializer.Deserialize<Contracts.GetCustomer.CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        UpdateCustomerDto updateCustomerDto = MapToUpdateCustomerDto(customer);

        response = await this._httpClient.PutAsync(
            "/api/customers",
            new StringContent(
                JsonSerializer.Serialize(updateCustomerDto,
                    this._jsonSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"
            ));

        // Assert

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCustomer_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Arrange

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/customers?firstName=Bob&lastName=Smith");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCustomer.CustomerDto customer =
            JsonSerializer.Deserialize<Contracts.GetCustomer.CustomerDto>(body, this._jsonSerializerOptions);

        // Act

        UpdateCustomerDto updateCustomerDto = new(
            "Bob2",
            "Smith2",
            customer.CardNumber,
            customer.SecurityNumber,
            customer.Expiration,
            customer.CardHolderName,
            customer.CardType,
            customer.Street,
            customer.City,
            customer.State,
            customer.Country,
            customer.ZipCode
        );

        response = await this._httpClient.PutAsync(
            "/api/customers",
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

        // Act

        HttpResponseMessage response = await this._httpClient.DeleteAsync(
            "/api/customers?firstName=Alice&lastName=Smith");

        // Assert

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteCustomer_ReturnNotFoundGivenCustomerDoesNotExist()
    {
        // Arrange

        // Act

        HttpResponseMessage response = await this._httpClient.DeleteAsync(
            "/api/customers?firstName=Bob2&lastName=Smith2");

        // Assert

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static UpdateCustomerDto MapToUpdateCustomerDto(Contracts.GetCustomer.CustomerDto customer)
    {
        return new UpdateCustomerDto(
            customer.FirstName,
            customer.LastName,
            customer.CardNumber,
            customer.SecurityNumber,
            customer.Expiration,
            customer.CardHolderName,
            customer.CardType,
            customer.Street,
            customer.City,
            customer.State,
            customer.Country,
            customer.ZipCode
        );
    }
}
