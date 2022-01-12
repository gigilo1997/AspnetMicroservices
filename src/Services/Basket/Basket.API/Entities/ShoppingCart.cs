namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {
        Items = new List<ShoppingCartItem>();
    }

    public ShoppingCart(string username) : this()
    {
        Username = username;
    }

    public string Username { get; set; }
    public List<ShoppingCartItem> Items { get; set; }

    public decimal TotalPrice => Items?.Sum(e => e.Quantity * e.Price) ?? 0;
}
