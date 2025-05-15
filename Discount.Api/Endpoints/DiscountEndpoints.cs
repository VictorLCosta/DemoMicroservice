using Discount.Api.Entities;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Discount.Api.Endpoints;

public static class DiscountEndpoints
{
    public static void MapDiscountEndpoints(this WebApplication app)
    {
        var discount = app.MapGroup("/discounts");

        discount.MapGet("/{productName}", GetDiscount);

        discount.MapPost("/", AddDiscount);

        discount.MapPut("/", UpdateDiscount);

        discount.MapDelete("/{productName}", DeleteDiscount);
    }

    private static async Task<Results<Ok<Coupon>, NotFound>> GetDiscount(string productName, IDiscountRepository repository) =>
        await repository.GetDiscount(productName) is { } coupon
            ? TypedResults.Ok(coupon)
            : TypedResults.NotFound();

    private static async Task<Results<Created<Coupon>, BadRequest<string>>> AddDiscount(Coupon coupon, IDiscountRepository repository)
    {
        if (string.IsNullOrWhiteSpace(coupon.ProductName))
            return TypedResults.BadRequest("Product name is required");

        if (coupon.Amount <= 0)
            return TypedResults.BadRequest("Discount must be greater than zero");

        await repository.CreateDiscount(coupon);

        return TypedResults.Created($"/discounts/{coupon.ProductName}", coupon);
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> UpdateDiscount(Coupon coupon, IDiscountRepository repository)
    {
        if (string.IsNullOrWhiteSpace(coupon.ProductName))
            return TypedResults.BadRequest("Product name is required");

        if (coupon.Amount <= 0)
            return TypedResults.BadRequest("Discount must be greater than zero");

        if (await repository.GetDiscount(coupon.ProductName) is null)
            return TypedResults.NotFound();

        await repository.UpdateDiscount(coupon);

        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteDiscount(string productName, IDiscountRepository repository)
    {
        if (await repository.GetDiscount(productName) is null)
            return TypedResults.NotFound();

        await repository.DeleteDiscount(productName);

        return TypedResults.NoContent();
    }
}
