using Bogus;

using MongoDB.Bson;

namespace Catalog.Api.Data;

public class CatalogContextSeed(ICatalogContext context)
{
    private readonly ICatalogContext _context = context;

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        try
        {
            await TrySeedAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task TrySeedAsync(CancellationToken cancellationToken)
    {
        if (await _context.Products.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken) == 0)
        {
            var products = new Faker<Product>()
                .RuleFor(x => x.Id, f => ObjectId.GenerateNewId().ToString())
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(x => x.ImageFile, f => f.Image.PicsumUrl())
                .Generate(10);

            await _context.Products.InsertManyAsync(products, cancellationToken: cancellationToken);
        }
    }
}
