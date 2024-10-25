namespace eShop.Catalog.API.Application.Exceptions;

internal class CatalogTypesNotFoundException : Exception
{
    public CatalogTypesNotFoundException() : base("Catalog types not found")
    {
    }
}
