namespace Catalog.Api.Extensions;

public static class Extensions
{
    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<CatalogContextSeed>()
            .SeedAsync(cancellationToken);
    }
}
