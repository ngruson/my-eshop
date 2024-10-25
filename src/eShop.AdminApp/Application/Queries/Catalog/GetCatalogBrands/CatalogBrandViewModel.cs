using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogBrands;

public record CatalogBrandViewModel(string objectId, string name)
{
    public string ObjectId { get; init; } = objectId;

    [Display(Name = "Brand")]
    public string Name { get; init; } = name;
}
