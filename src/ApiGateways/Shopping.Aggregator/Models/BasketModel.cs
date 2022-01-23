namespace Shopping.Aggregator.Models;

public class BasketModel
{
    public BasketModel()
    {
        Items = new List<BasketItemExtendedModel>();
    }

    public string Username { get; set; }
    public List<BasketItemExtendedModel> Items { get; set; }
    public decimal TotalPrice { get; set; }
}
