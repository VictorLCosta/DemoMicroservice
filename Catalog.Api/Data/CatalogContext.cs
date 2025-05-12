using Catalog.Api.Config;

using Microsoft.Extensions.Options;

namespace Catalog.Api.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        Products = mongoDatabase.GetCollection<Product>(
            databaseSettings.Value.BooksCollectionName);
    }

    public IMongoCollection<Product> Products { get; }
}
