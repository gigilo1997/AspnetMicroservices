using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public record GetOrdersListQuery(string Username) : IRequest<List<OrdersVm>>;
