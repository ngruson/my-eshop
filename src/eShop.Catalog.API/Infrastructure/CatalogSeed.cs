using System.Text.Json;
using eShop.Catalog.API.Services;
using eShop.Shared.Data;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;
using Pgvector;

namespace eShop.Catalog.API.Infrastructure;

public partial class CatalogSeed : IDbSeeder
{
    private readonly ICatalogAI _catalogAI;
    private readonly ILogger<CatalogSeed> _logger;    

    public CatalogSeed(IWebHostEnvironment env,
        ICatalogAI catalogAI,
        ILogger<CatalogSeed> logger)
    {
        this._catalogAI = catalogAI;
        this._logger = logger;

        string contentRootPath = env.ContentRootPath;
        string sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
        string sourceJson = File.ReadAllText(sourcePath);
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
                .Select(brandName => new CatalogBrand(Guid.NewGuid(), brandName)));
            IEnumerable<CatalogBrand> addedBrands = await catalogBrandRepository.ListAsync();
            this._logger.LogInformation("Seeded catalog with {NumBrands} brands", addedBrands.Count());

            await catalogTypeRepository.DeleteRangeAsync(await catalogTypeRepository.ListAsync());
            await catalogTypeRepository.AddRangeAsync(this.SourceItems!.Select(x => x.Type).Distinct()
                .Select(typeName => new CatalogType(Guid.NewGuid(), typeName)));
            IEnumerable<CatalogType> addedTypes = await catalogTypeRepository.ListAsync();
            this._logger.LogInformation("Seeded catalog with {NumTypes} types", addedTypes.Count());

            CatalogItem[] catalogItems = this.SourceItems!.Select(source => new CatalogItem(
                Guid.NewGuid(),
                source.Name,
                source.Description,
                source.Price,
                $"{source.Id}.webp",
                addedTypes.Single(x => x.Type == source.Type),
                addedBrands.Single(x => x.Brand == source.Brand),
                100,
                10,
                200))
            .ToArray();

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
