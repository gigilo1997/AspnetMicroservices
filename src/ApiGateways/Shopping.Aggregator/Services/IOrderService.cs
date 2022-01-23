using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrders(string username);
}