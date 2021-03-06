using AspnetRunBasics.Services;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages;

public class OrderModel : PageModel
{
    private readonly IOrderService _orderService;

    public OrderModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public IEnumerable<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        string username = "swn";
        Orders = await _orderService.GetOrdersByUserName(username);

        return Page();
    }
}
