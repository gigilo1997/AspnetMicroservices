using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages;

public class ProductDetailModel : PageModel
{
    private readonly ICatalogueService _catalogueService;
    private readonly IBasketService _basketService;

    public ProductDetailModel(ICatalogueService catalogueService, IBasketService basketService)
    {
        _catalogueService = catalogueService;
        _basketService = basketService;
    }

    public CatalogueModel Product { get; set; }

    [BindProperty]
    public string Color { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    public async Task<IActionResult> OnGetAsync(string productId)
    {
        if (productId == null)
        {
            return NotFound();
        }

        Product = await _catalogueService.GetCatalogue(productId);
        if (Product == null)
        {
            return NotFound();
        }
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
                Color = Color,
                Quantity = Quantity
            });
        }

        await _basketService.UpdateBasket(basket);
        return RedirectToPage("Cart");
    }
}
