using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages;

public class ProductModel : PageModel
{
    private readonly ICatalogueService _catalogueService;
    private readonly IBasketService _basketService;

    public ProductModel(ICatalogueService catalogueService, IBasketService basketService)
    {
        _catalogueService = catalogueService;
        _basketService = basketService;
    }

    public IEnumerable<string> CategoryList { get; set; } = new List<string>();
    public IEnumerable<CatalogueModel> ProductList { get; set; } = new List<CatalogueModel>();


    [BindProperty(SupportsGet = true)]
    public string SelectedCategory { get; set; }

    public async Task<IActionResult> OnGetAsync(string category)
    {
        var products = await _catalogueService.GetCatalogue();
        CategoryList = products.Select(p => p.Category).Distinct();

        if (!string.IsNullOrWhiteSpace(category))
        {
            ProductList = products.Where(p => p.Category == category);
            SelectedCategory = category;
        }
        else
        {
            ProductList = products;
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
                Color = "Black",
                Quantity = 1
            });
        }

        await _basketService.UpdateBasket(basket);
        return RedirectToPage("Cart");
    }
}
