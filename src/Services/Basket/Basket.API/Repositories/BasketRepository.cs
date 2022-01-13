using Basket.API.Entities;
using Basket.API.GrpcServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketRepository(IDistributedCache cache, DiscountGrpcService discountGrpcService)
    {
        _cache = cache;
        _discountGrpcService = discountGrpcService;
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
        if (basket is null)
            return null;

        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        string jsonBasket = JsonSerializer.Serialize(basket);
        await _cache.SetStringAsync(basket.Username, jsonBasket);

        return await GetBasket(basket.Username);
    }

    public async Task DeleteBasket(string username)
    {
        await _cache.RemoveAsync(username);
    }
}
