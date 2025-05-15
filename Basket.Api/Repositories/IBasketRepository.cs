using Basket.Api.Entities;

namespace Basket.Api.Repositories;

public interface IBasketRepository
{
    public Task DeleteBasket(string userName);
    public Task<ShoppingCart?> GetBasket(string userName);
    public Task<ShoppingCart?> UpdateBasket(ShoppingCart basket);
}
