using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

public record CatalogTypeViewModel(Guid objectId, string name)
{
    public Guid ObjectId { get; init; } = objectId;

    [Display(Name = "Type")]
    public string Name { get; init; } = name;
}
