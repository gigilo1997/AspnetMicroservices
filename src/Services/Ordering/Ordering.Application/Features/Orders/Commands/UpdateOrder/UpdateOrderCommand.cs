using MediatR;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand : IRequest
{
    public int Id { get; set; }
    public string Username { get; init; }
    public decimal TotalPrice { get; init; }

    // BillingAddress
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string EmailAddress { get; init; }
    public string AddressLine { get; init; }
    public string Country { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }

    // Payment
    public string CardName { get; init; }
    public string CardNumber { get; init; }
    public string Expiration { get; init; }
    public string CVV { get; init; }
    public int PaymentMethod { get; init; }
}
