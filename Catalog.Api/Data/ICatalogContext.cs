namespace Catalog.Api.Data;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
}
