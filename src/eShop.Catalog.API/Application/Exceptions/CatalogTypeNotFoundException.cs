namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogTypeNotFoundException : Exception
{
    public CatalogTypeNotFoundException() : base("Catalog type not found")
    {
    }
}
