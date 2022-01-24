using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;

public interface IBasketService
{
    Task<BasketModel> GetBasket(string username);
    Task<BasketModel> UpdateBasket(BasketModel model);
    Task CheckoutBasket(BasketCheckoutModel model);
}
