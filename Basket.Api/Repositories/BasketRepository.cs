using System.Text.Json;

using Basket.Api.Entities;

using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repositories;

public class BasketRepository(IDistributedCache distributedCache) : IBasketRepository
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task DeleteBasket(string userName)
    {
        await _distributedCache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await _distributedCache.GetStringAsync(userName);

        if (string.IsNullOrEmpty(basket))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ShoppingCart>(basket);

    }

    public async Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
    {
        await _distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

        return await GetBasket(basket.UserName);
    }
}
