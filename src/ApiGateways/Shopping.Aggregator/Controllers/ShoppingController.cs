using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public ShoppingController(ICatalogueService catalogueService, IBasketService basketService, IOrderService orderService)
    {
        _catalogueService = catalogueService;
        _basketService = basketService;
        _orderService = orderService;
    }

    [HttpGet("{username}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
    {
        var model = new ShoppingModel { Username = username };
        model.BasketWithProducts = await _basketService.GetBasket(username);

        foreach (var item in model.BasketWithProducts.Items)
        {
            try
            {
                var product = await _catalogueService.GetCatalogueById(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }

        model.Orders = await _orderService.GetOrders(username);

        return Ok(model);
    }
}
