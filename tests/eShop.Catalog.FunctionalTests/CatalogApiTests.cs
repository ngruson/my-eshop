using System.Net.Http.Json;
using System.Text.Json;
using Asp.Versioning;
using Asp.Versioning.Http;
using eShop.Catalog.API.Application.Queries.GetCatalogItemPictureByObjectId;
using eShop.Catalog.API.Model;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace eShop.Catalog.FunctionalTests;

public sealed class CatalogApiTests : IClassFixture<CatalogApiFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public CatalogApiTests(CatalogApiFixture fixture)
    {
        ApiVersionHandler handler = new(new QueryStringApiVersionWriter(), new ApiVersion(1.0));

        this._webApplicationFactory = fixture;
        this._httpClient = this._webApplicationFactory.CreateDefaultClient(handler);
    }

    [Fact]
    public async Task GetCatalogItemsRespectsPageSize()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("/api/catalog/items/page?pageIndex=0&pageSize=5");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert

        Assert.Equal(5, result.Data.Length);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task UpdateCatalogItemWorksWithoutPriceUpdate()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> catalogItems =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");
        Contracts.GetCatalogItems.CatalogItemDto catalogItem = catalogItems.Data.FirstOrDefault();

        // Act - 1

        HttpResponseMessage response = await this._httpClient.GetAsync($"/api/catalog/items/{catalogItem.ObjectId}");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItem.CatalogItemDto itemToUpdate =
            JsonSerializer.Deserialize<Contracts.GetCatalogItem.CatalogItemDto>(body, this._jsonSerializerOptions);

        // Act - 2
        
        Contracts.UpdateCatalogItem.CatalogItemDto updateItem = new(
            itemToUpdate.Name,
            itemToUpdate.Description,
            itemToUpdate.Price,
            itemToUpdate.PictureUrl,
            itemToUpdate.CatalogType.ObjectId,
            itemToUpdate.CatalogBrand.ObjectId,
            itemToUpdate.AvailableStock - 1,
            itemToUpdate.RestockThreshold,
            itemToUpdate.MaxStockThreshold,
            itemToUpdate.OnReorder);

        response = await this._httpClient.PutAsJsonAsync($"/api/catalog/items/{itemToUpdate.ObjectId}", updateItem);
        response.EnsureSuccessStatusCode();

        // Act - 3
        response = await this._httpClient.GetAsync($"/api/catalog/items/{itemToUpdate.ObjectId}");
        response.EnsureSuccessStatusCode();
        body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItem.CatalogItemDto updatedItem =
            JsonSerializer.Deserialize<Contracts.GetCatalogItem.CatalogItemDto>(body, this._jsonSerializerOptions);

        // Assert

        Assert.Equal(itemToUpdate.ObjectId, updatedItem.ObjectId);
        Assert.NotEqual(itemToUpdate.AvailableStock, updatedItem.AvailableStock);
    }

    [Fact]
    public async Task UpdateCatalogItemWorksWithPriceUpdate()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> catalogItems =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");
        Contracts.GetCatalogItems.CatalogItemDto catalogItem = catalogItems.Data.FirstOrDefault();

        // Act - 1

        HttpResponseMessage response = await this._httpClient.GetAsync($"/api/catalog/items/{catalogItem.ObjectId}");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItem.CatalogItemDto itemToUpdate =
            JsonSerializer.Deserialize<Contracts.GetCatalogItem.CatalogItemDto>(body, this._jsonSerializerOptions);

        // Act - 2
        
        Contracts.UpdateCatalogItem.CatalogItemDto updateItem = new(
            itemToUpdate.Name,
            itemToUpdate.Description,
            1.99m,
            itemToUpdate.PictureUrl,
            itemToUpdate.CatalogType.ObjectId,
            itemToUpdate.CatalogBrand.ObjectId,            
            itemToUpdate.AvailableStock - 1,
            itemToUpdate.RestockThreshold,
            itemToUpdate.MaxStockThreshold,
            itemToUpdate.OnReorder);

        response = await this._httpClient.PutAsJsonAsync($"/api/catalog/items/{itemToUpdate.ObjectId}", updateItem);
        response.EnsureSuccessStatusCode();

        // Act - 3
        response = await this._httpClient.GetAsync($"/api/catalog/items/{itemToUpdate.ObjectId}");
        response.EnsureSuccessStatusCode();
        body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItem.CatalogItemDto updatedItem =
            JsonSerializer.Deserialize<Contracts.GetCatalogItem.CatalogItemDto>(body, this._jsonSerializerOptions);

        // Assert - 1
        Assert.Equal(itemToUpdate.ObjectId, updatedItem.ObjectId);
        Assert.Equal(1.99m, updatedItem.Price);
        Assert.NotEqual(itemToUpdate.AvailableStock, updatedItem.AvailableStock);
    }

    [Fact]
    public async Task GetCatalogItemsByIds()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> page =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync(
            $"/api/catalog/items/by?ids={page.Data[0].ObjectId}&ids={page.Data[1].ObjectId}&ids={page.Data[2].ObjectId}");        
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItems.CatalogItemDto[] result =
            JsonSerializer.Deserialize<Contracts.GetCatalogItems.CatalogItemDto[]>(body, this._jsonSerializerOptions);

        // Assert

        Assert.Equal(3, result.Length);
    }

    [Fact]
    public async Task GetCatalogItemWithId()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> page =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync($"/api/catalog/items/{page.Data[0].ObjectId}");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        Contracts.GetCatalogItems.CatalogItemDto result =
            JsonSerializer.Deserialize<Contracts.GetCatalogItems.CatalogItemDto>(body, this._jsonSerializerOptions);

        // Assert       
        
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCatalogItemWithExactName()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("api/catalog/items/by/Wanderer%20Black%20Hiking%20Boots?PageSize=5&PageIndex=0");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert

        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Count);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
        Assert.Equal("Wanderer Black Hiking Boots", result.Data.ToList().FirstOrDefault().Name);
    }

    // searching partial name Alpine
    [Fact]
    public async Task GetCatalogItemWithPartialName()
    {
        // Act
        HttpResponseMessage response = await this._httpClient.GetAsync("api/catalog/items/by/Alpine?PageSize=5&PageIndex=0");

        // Arrange   
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(4, result.Count);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
        Assert.Contains("Alpine", result.Data.ToList().FirstOrDefault().Name);
    }


    [Fact]
    public async Task GetCatalogItemPicWithId()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> catalogItems =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");
        Contracts.GetCatalogItems.CatalogItemDto catalogItem = catalogItems.Data.FirstOrDefault();

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync($"api/catalog/items/{catalogItem.ObjectId}/pic");
        response.EnsureSuccessStatusCode();

        // Assert

        Assert.True(IsWebP(response.Content.ReadAsStream()));
    }

    private static bool IsWebP(Stream stream)
    {
        if (stream == null || !stream.CanRead)
        {
            throw new ArgumentException("Stream is null or not readable.");
        }

        // WebP magic number for RIFF header
        byte[] riffHeader = new byte[4];
        stream.ReadExactly(riffHeader, 0, 4);
        string riffHeaderString = System.Text.Encoding.ASCII.GetString(riffHeader);

        if (riffHeaderString != "RIFF")
        {
            return false;
        }

        // Skip 4 bytes (file size)
        stream.Seek(4, SeekOrigin.Current);

        // WebP magic number for format header
        byte[] webPHeader = new byte[4];
        stream.ReadExactly(webPHeader, 0, 4);
        string webPHeaderString = System.Text.Encoding.ASCII.GetString(webPHeader);

        return webPHeaderString == "WEBP";
    }


    [Fact]
    public async Task GetCatalogItemsWithSemanticRelevance()
    {
        // Act
        HttpResponseMessage response = await this._httpClient.GetAsync("api/catalog/items/withSemanticRelevance/Wanderer?PageSize=5&PageIndex=0");        
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert       
        Assert.Equal(1, result.Count);
        Assert.NotNull(result.Data);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task GetCatalogItemWithTypeIdBrandId()
    {
        // Arrange

        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> catalogItems =
            await this._httpClient.GetFromJsonAsync<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>("/api/catalog/items/page?pageIndex=0&pageSize=5");
        Contracts.GetCatalogItems.CatalogItemDto catalogItem = catalogItems.Data.FirstOrDefault();

        // Act

        HttpResponseMessage response = await this._httpClient
            .GetAsync($"api/catalog/items/type/{catalogItem.CatalogType.ObjectId}/brand/{catalogItem.CatalogBrand.ObjectId}?PageSize=5&PageIndex=0");

        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert    
        Assert.NotNull(result.Data);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task GetAllCatalogTypeItemWithBrandId()
    {
        // Arrange

        CatalogBrandDto[] brands = await this._httpClient.GetFromJsonAsync<CatalogBrandDto[]>("api/catalog/catalogBrands");

        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync($"api/catalog/items/type/all/brand/{brands[0].ObjectId}?PageSize=5&PageIndex=0");
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto> result =
            JsonSerializer.Deserialize<PaginatedItems<Contracts.GetCatalogItems.CatalogItemDto>>(body, this._jsonSerializerOptions);

        // Assert

        Assert.NotNull(result.Data);
        Assert.Equal(5, result.Data.Length);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task GetAllCatalogTypes()
    {
        // Act

        HttpResponseMessage response = await this._httpClient.GetAsync("api/catalog/catalogTypes");

        // Arrange

        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        CatalogType[] result = JsonSerializer.Deserialize<CatalogType[]>(body, this._jsonSerializerOptions);

        // Assert

        Assert.Equal(8, result.Length);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllCatalogBrands()
    {
        // Act
        HttpResponseMessage response = await this._httpClient.GetAsync("api/catalog/catalogBrands");

        // Arrange   
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        List<CatalogBrand> result = JsonSerializer.Deserialize<List<CatalogBrand>>(body, this._jsonSerializerOptions);

        // Assert       
        Assert.Equal(13, result.Count);
        Assert.NotNull(result);
    }

    [Theory, AutoNSubstituteData]
    public async Task AddCatalogItem(
        CreateCatalogItemDto dto)
    {
        // Act - 1

        CatalogBrandDto[] brands = await this._httpClient.GetFromJsonAsync<CatalogBrandDto[]>("api/catalog/catalogBrands");
        CatalogTypeDto[] types = await this._httpClient.GetFromJsonAsync<CatalogTypeDto[]>("api/catalog/catalogTypes");

        // Act - 2

        HttpResponseMessage response = await this._httpClient.PostAsJsonAsync("/api/catalog/items",
            dto with { CatalogBrand = brands[0].ObjectId, CatalogType = types[0].ObjectId });
        response.EnsureSuccessStatusCode();

        Stream stream = await response.Content.ReadAsStreamAsync();
        CatalogItemDto addedItem = JsonSerializer.Deserialize<CatalogItemDto>(stream, this._jsonSerializerOptions);

        // Assert - 1
        Assert.Equal(dto.Name, addedItem.Name);
    }

    [Theory, AutoNSubstituteData]
    public async Task DeleteCatalogItem(
        CreateCatalogItemDto dto)
    {
        // Arrange

        CatalogBrandDto[] brands = await this._httpClient.GetFromJsonAsync<CatalogBrandDto[]>("api/catalog/catalogBrands");
        CatalogTypeDto[] types = await this._httpClient.GetFromJsonAsync<CatalogTypeDto[]>("api/catalog/catalogTypes");

        HttpResponseMessage response = await this._httpClient.PostAsJsonAsync("/api/catalog/items",
            dto with { CatalogBrand = brands[0].ObjectId, CatalogType = types[0].ObjectId });
        Stream stream = await response.Content.ReadAsStreamAsync();
        CatalogItemDto addedItem = JsonSerializer.Deserialize<CatalogItemDto>(stream, this._jsonSerializerOptions);

        // Act

        response = await this._httpClient.DeleteAsync($"/api/catalog/items/{addedItem.ObjectId}");
        response.EnsureSuccessStatusCode();

        // Assert

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
