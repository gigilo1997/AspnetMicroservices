using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages;

public class IndexModel : PageModel
{
    private readonly ICatalogueService _catalogueService;
    private readonly IBasketService _basketService;

    public IndexModel(ICatalogueService catalogueService, IBasketService basketService)
    {
        _catalogueService = catalogueService;
        _basketService = basketService;
    }

    public IEnumerable<CatalogueModel> ProductList { get; set; } = new List<CatalogueModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        ProductList = await _catalogueService.GetCatalogue();
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        var username = "swn";
        var product = await _catalogueService.GetCatalogue(productId);
        var basket = await _basketService.GetBasket(username);

        var basketItem = basket.Items.FirstOrDefault(e => e.ProductId == productId);

        if (basketItem != null)
        {
            basketItem.Quantity++;
        }
        else
        {
            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Color = "Black",
                Quantity = 1
            });
        }

        await _basketService.UpdateBasket(basket);
        return RedirectToPage("Cart");
    }
}
