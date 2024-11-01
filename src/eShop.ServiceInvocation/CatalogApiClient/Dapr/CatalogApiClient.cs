using Dapr.Client;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Shared.Auth;
using eShop.Shared.Data;

namespace eShop.ServiceInvocation.CatalogApiClient.Dapr;

public class CatalogApiClient(DaprClient daprClient, AccessTokenAccessor accessTokenAccessor)
    : BaseDaprApiClient(daprClient, accessTokenAccessor), ICatalogApiClient
{
    private readonly string basePath = "api/catalog";
    protected override string AppId => "catalog-api";

    public async Task CreateCatalogItem(CreateCatalogItemDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/items",
            null,
            dto);

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task DeleteCatalogItem(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Delete,
            $"{this.basePath}/items/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task<Catalog.Contracts.GetCatalogBrands.CatalogBrandDto[]> GetBrands()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/catalogBrands");

        return await this.DaprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogBrands.CatalogBrandDto[]>(request);
    }

    public async Task<CatalogItemViewModel> GetCatalogItem(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/items/{objectId}");

        Catalog.Contracts.GetCatalogItem.CatalogItemDto response =
            await this.DaprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItem.CatalogItemDto>(request);

        return response.Map();
    }

    public async Task<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]> GetCatalogItems(bool includeDeleted = false)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/items",
            [new KeyValuePair<string, string>("includeDeleted", includeDeleted.ToString())]);

        return await this.DaprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]>(request);
    }

    public async Task<CatalogItemViewModel[]> GetCatalogItems(Guid[] ids)
    {
        KeyValuePair<string, string>[] queryStringParameters = ids
            .Select(id =>
                new KeyValuePair<string, string>("ids", id.ToString())).ToArray();

        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/items",
            queryStringParameters);

        Catalog.Contracts.GetCatalogItems.CatalogItemDto[] response =
            await this.DaprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]>(request);

        return response.Map();
    }

    public async Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItems(Guid? catalogType, Guid? catalogBrand, int pageSize, int pageIndex)
    {
        PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems;
        HttpRequestMessage request;

        if (catalogType.HasValue)
        {
            request = await this.CreateRequest(
                HttpMethod.Get,
                $"{this.basePath}/items/type/{catalogType}/brand/{catalogBrand}",
                [
                    new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("pageIndex", pageIndex.ToString())
                ]);
        }
        else if (catalogBrand.HasValue)
        {
            request = await this.CreateRequest(
                HttpMethod.Get,
                $"{this.basePath}/items/type/all/brand/{catalogBrand}",
                [
                    new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("pageIndex", pageIndex.ToString())
                ]);
        }
        else
        {
            request = await this.CreateRequest(
                HttpMethod.Get,
                $"{this.basePath}/items/page",
                [
                    new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("pageIndex", pageIndex.ToString())
                ]);
        }

        paginatedItems = await this.DaprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(request);

        return paginatedItems.Map();
    }

    public async Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItemsWithSemanticRelevance(string text, int pageSize, int pageIndex)
    {
        HttpRequestMessage request = await this.CreateRequest(
                HttpMethod.Get,
                $"{this.basePath}/items/withSemanticRelevance/{text}",
                [
                    new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("pageIndex", pageIndex.ToString())
                ]);

        PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems =
            await this.DaprClient.InvokeMethodAsync<PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto>>(request);

        return paginatedItems.Map();
    }

    public async Task<Catalog.Contracts.GetCatalogTypes.CatalogTypeDto[]> GetTypes()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/catalogTypes");

        return await this.DaprClient.InvokeMethodAsync<Catalog.Contracts.GetCatalogTypes.CatalogTypeDto[]>(request);
    }

    public async Task UpdateCatalogItem(Guid objectId, Catalog.Contracts.UpdateCatalogItem.CatalogItemDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Put,
            $"{this.basePath}/items/{objectId}",
            null,
            dto);

        await this.DaprClient.InvokeMethodAsync(request);
    }
}
