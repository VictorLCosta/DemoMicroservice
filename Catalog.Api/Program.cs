using Catalog.Api.Config;
using Catalog.Api.Extensions;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddTransient<CatalogContextSeed>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api-docs");

    await app.Services.InitializeDatabasesAsync();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
