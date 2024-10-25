using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Catalog.API.Model;

public class CatalogType : Entity, IAggregateRoot
{    
    public CatalogType(Guid objectId, string type)
    {
        this.ObjectId = objectId;
        this.Type = type;
    }

    [Required]
    public string Type { get; set; }
}
