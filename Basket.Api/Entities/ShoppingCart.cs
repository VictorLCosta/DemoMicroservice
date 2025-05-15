namespace Basket.Api.Entities;

public class ShoppingCart
{
    public string UserName { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}
