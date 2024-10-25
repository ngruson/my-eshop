using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Catalog.API.Model;

public class CatalogBrand : Entity, IAggregateRoot
{
    public CatalogBrand(Guid objectId, string brand)
    {
        this.ObjectId = objectId;
        this.Brand = brand;
    }
    [Required]
    public string Brand { get; set; }
}
