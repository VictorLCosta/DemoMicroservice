using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api-docs");
}

app.MapDiscountEndpoints();

app.UseHttpsRedirection();

await app.RunAsync();
