using System.Net.Http.Json;
using System.Web;
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    private readonly string remoteServiceBaseUrl = "api/catalog/";

    public Task<CatalogItem?> GetCatalogItem(int id)
    {
        string uri = $"{this.remoteServiceBaseUrl}items/{id}";
        return httpClient.GetFromJsonAsync<CatalogItem>(uri);
    }

    public async Task<CatalogItem[]> GetCatalogItems()
    {
        string uri = $"{this.remoteServiceBaseUrl}items";
        CatalogItem[]? result = await httpClient.GetFromJsonAsync<CatalogItem[]>(uri);
        return result!;
    }

    public async Task<CatalogResult> GetPaginatedCatalogItems(int pageIndex, int pageSize, int? brand, int? type)
    {
        string uri = GetPaginatedCatalogItemsUri(this.remoteServiceBaseUrl, pageIndex, pageSize, brand, type);
        CatalogResult? result = await httpClient.GetFromJsonAsync<CatalogResult>(uri);
        return result!;
    }

    public async Task<CatalogItem[]> GetCatalogItems(IEnumerable<int> ids)
    {
        string uri = $"{this.remoteServiceBaseUrl}items/by?ids={string.Join("&ids=", ids)}";
        CatalogItem[]? result = await httpClient.GetFromJsonAsync<CatalogItem[]>(uri);
        return result!;
    }

    public async Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text)
    {
        string url = $"{this.remoteServiceBaseUrl}items/withsemanticrelevance/{HttpUtility.UrlEncode(text)}?pageIndex={page}&pageSize={take}";
        CatalogResult? result = await httpClient.GetFromJsonAsync<CatalogResult>(url);
        return result!;
    }

    public async Task<CatalogBrand[]> GetBrands()
    {
        string uri = $"{this.remoteServiceBaseUrl}catalogBrands";
        CatalogBrand[]? result = await httpClient.GetFromJsonAsync<CatalogBrand[]>(uri);
        return result!;
    }

    public async Task<CatalogItemType[]> GetTypes()
    {
        string uri = $"{this.remoteServiceBaseUrl}catalogTypes";
        CatalogItemType[]? result = await httpClient.GetFromJsonAsync<CatalogItemType[]>(uri);
        return result!;
    }

    private static string GetPaginatedCatalogItemsUri(string baseUri, int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs;

        if (type.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/{type.Value}/brand/{brandQs}";

        }
        else if (brand.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/all/brand/{brandQs}";
        }
        else
        {
            filterQs = string.Empty;
        }

        return $"{baseUri}items/page/{filterQs}?pageIndex={pageIndex}&pageSize={pageSize}";
    }
}
