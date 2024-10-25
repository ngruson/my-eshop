namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogItemsNotFoundException : Exception
{
    public CatalogItemsNotFoundException() : base("Catalog items not found")
    {
    }
}
