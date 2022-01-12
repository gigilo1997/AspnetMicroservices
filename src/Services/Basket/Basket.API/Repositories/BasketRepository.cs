using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;

    public BasketRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<ShoppingCart> GetBasket(string username)
    {
        string jsonBasket = await _cache.GetStringAsync(username);
        if (string.IsNullOrEmpty(jsonBasket))
            return null;

        return JsonSerializer.Deserialize<ShoppingCart>(jsonBasket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        if (basket is null || string.IsNullOrEmpty(basket.Username))
            return null;

        string jsonBasket = JsonSerializer.Serialize(basket);
        await _cache.SetStringAsync(basket.Username, jsonBasket);

        return await GetBasket(basket.Username);
    }

    public async Task DeleteBasket(string username)
    {
        await _cache.RemoveAsync(username);
    }
}
