using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        this._basketRepository = basketRepository;
        this._publishEndpoint = publishEndpoint;
        this._mapper = mapper;
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
    {
        var basket = await _basketRepository.GetBasket(username);
        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        return Ok(await _basketRepository.UpdateBasket(basket));
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteBasket(string username)
    {
        await _basketRepository.DeleteBasket(username);
        return Ok();
    }

    [HttpPost("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Checkout([FromBody] BasketCheckout checkout)
    {
        var basket = await _basketRepository.GetBasket(checkout.Username);
        if (basket == null)
            return BadRequest();

        var eventMessage = _mapper.Map<BasketCheckoutEvent>(checkout);
        eventMessage.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);

        await _basketRepository.DeleteBasket(basket.Username);

        return Accepted();
    }
}
