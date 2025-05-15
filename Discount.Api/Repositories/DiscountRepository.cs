using Dapper;

using Discount.Api.Entities;

using Npgsql;

namespace Discount.Api.Repositories;

public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var cn = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDb"));

        var affected = await cn.ExecuteAsync("INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", coupon);

        return affected > 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var cn = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDb"));

        var affected = await cn.ExecuteAsync("DELETE FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

        return affected > 0;
    }

    public async Task<Coupon?> GetDiscount(string productName)
    {
        using var cn = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDb"));

        var coupon = await cn.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

        return coupon;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var cn = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDb"));

        var affected = await cn.ExecuteAsync("UPDATE Coupons SET Description = @Description, Amount = @Amount WHERE ProductName = @ProductName", coupon);

        return affected > 0;
    }
}
