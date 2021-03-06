using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages;

public class CartModel : PageModel
{
    private readonly IBasketService _basketService;

    public CartModel(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public BasketModel Cart { get; set; } = new BasketModel();

    public async Task<IActionResult> OnGetAsync()
    {
        string username = "swn";
        Cart = await _basketService.GetBasket(username);

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
    {
        var username = "swn";
        var basket = await _basketService.GetBasket(username);

        var item = basket.Items.FirstOrDefault(p => p.ProductId == productId);

        if (item != null)
            basket.Items.Remove(item);

        await _basketService.UpdateBasket(basket);
        return RedirectToPage();
    }
}
