using System.Text.Json;
using eShop.Catalog.API.Services;
using eShop.Shared.Data;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;
using Pgvector;

namespace eShop.Catalog.API.Infrastructure;

public partial class CatalogSeed : IDbSeeder
{
    private readonly IWebHostEnvironment _env;
    private readonly ICatalogAI _catalogAI;
    private readonly ILogger<CatalogSeed> _logger;    

    public CatalogSeed(IWebHostEnvironment env,
        ICatalogAI catalogAI,
        ILogger<CatalogSeed> logger)
    {
        this._env = env;
        this._catalogAI = catalogAI;
        this._logger = logger;

        var contentRootPath = env.ContentRootPath;
        var sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
        var sourceJson = File.ReadAllText(sourcePath);
        this.SourceItems = JsonSerializer.Deserialize<CatalogSourceEntry[]>(sourceJson)!;
    }

    internal CatalogSourceEntry[] SourceItems { get; set; }

    public async Task SeedAsync(ServiceProviderWrapper services)
    {
        IRepository<CatalogItem> catalogItemRepository = services.GetRequiredService<IRepository<CatalogItem>>();
        IRepository<CatalogBrand> catalogBrandRepository = services.GetRequiredService<IRepository<CatalogBrand>>();
        IRepository<CatalogType> catalogTypeRepository = services.GetRequiredService<IRepository<CatalogType>>();

        if (!await catalogItemRepository.AnyAsync())
        {
            await catalogBrandRepository.DeleteRangeAsync(await catalogBrandRepository.ListAsync());
            await catalogBrandRepository.AddRangeAsync(this.SourceItems!.Select(x => x.Brand).Distinct()
                .Select(brandName => new CatalogBrand { Brand = brandName }));
            IEnumerable<CatalogBrand> addedBrands = await catalogBrandRepository.ListAsync();
            this._logger.LogInformation("Seeded catalog with {NumBrands} brands", addedBrands.Count());

            await catalogTypeRepository.DeleteRangeAsync(await catalogTypeRepository.ListAsync());
            await catalogTypeRepository.AddRangeAsync(this.SourceItems!.Select(x => x.Type).Distinct()
                .Select(typeName => new CatalogType { Type = typeName }));
            IEnumerable<CatalogType> addedTypes = await catalogTypeRepository.ListAsync();
            this._logger.LogInformation("Seeded catalog with {NumTypes} types", addedTypes.Count());

            var brandIdsByName = addedBrands.ToDictionary(x => x.Brand, x => x.Id);
            var typeIdsByName = addedTypes.ToDictionary(x => x.Type, x => x.Id);

            CatalogItem[] catalogItems = this.SourceItems!.Select(source => new CatalogItem
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                CatalogBrandId = brandIdsByName[source.Brand],
                CatalogTypeId = typeIdsByName[source.Type],
                AvailableStock = 100,
                MaxStockThreshold = 200,
                RestockThreshold = 10,
                PictureFileName = $"{source.Id}.webp",
            }).ToArray();

            if (this._catalogAI.IsEnabled)
            {
                this._logger.LogInformation("Generating {NumItems} embeddings", catalogItems.Length);
                IReadOnlyList<Vector>? embeddings = await this._catalogAI.GetEmbeddingsAsync(catalogItems);
                if (embeddings is not null)
                {
                    for (int i = 0; i < catalogItems.Length; i++)
                    {
                        catalogItems[i].Embedding = embeddings[i];
                    }
                }
            }

            await catalogItemRepository.AddRangeAsync(catalogItems);
            this._logger.LogInformation("Seeded catalog with {NumItems} items", catalogItems.Length);
        }
    }

    internal class CatalogSourceEntry
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}
