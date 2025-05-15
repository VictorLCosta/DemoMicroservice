using Basket.Api.Entities;
using Basket.Api.Repositories;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");    
});

var app = builder.Build();

var basket = app.MapGroup("/basket");

basket.MapGet("/{username:alpha}", async (string username, IBasketRepository repository) =>
{
    var basket = await repository.GetBasket(username);
    return basket ?? new ShoppingCart();
});

basket.MapDelete("/{username:alpha}", async (string username, IBasketRepository repository) =>
{
    await repository.DeleteBasket(username);
});

basket.MapPut("/", async (ShoppingCart basket, IBasketRepository repository) =>
{
    var updatedBasket = await repository.UpdateBasket(basket);
    return updatedBasket ?? new ShoppingCart();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api-docs");
}

app.UseHttpsRedirection();

app.Run();

