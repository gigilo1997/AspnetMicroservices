using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);
        var savedOrder = await _orderRepository.AddAsync(order);
        await SendMail(savedOrder);
        return savedOrder.Id;
    }

    private async Task SendMail(Order savedOrder)
    {
        var email = new Email {
            To = savedOrder.EmailAddress,
            Body = $"Order was created",
            Subject = "Order was created"
        };

        try
        {
            await _emailService.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {savedOrder.Id} failed due to an error with the mail service: {ex.Message}");
        }
    }
}
