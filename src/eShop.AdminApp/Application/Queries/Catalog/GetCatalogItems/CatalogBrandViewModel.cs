using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public record CatalogBrandViewModel(Guid objectId, string name)
{
    public Guid ObjectId { get; init; } = objectId;

    [Display(Name = "Brand")]
    public string Name { get; init; } = name;
}
