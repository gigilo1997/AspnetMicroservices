using Catalogue.API.Entities;
using Catalogue.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalogue.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogueController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CatalogueController> _logger;

    public CatalogueController(IProductRepository productRepository, ILogger<CatalogueController> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProductById")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _productRepository.GetProductById(id);
        if (product is null)
        {
            _logger.LogError($"Product with id: {id} was not found.");
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("[action]/{name}", Name = "GetProductByName")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductByName(string name)
    {
        var products = await _productRepository.GetProductsByName(name);
        return Ok(products);
    }

    [HttpGet("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductByCategory(string category)
    {
        var products = await _productRepository.GetProductsByCategory(category);
        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await _productRepository.CreateProduct(product);
        return CreatedAtRoute(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product)
    {
        var updateResult = await _productRepository.UpdateProduct(product);
        return Ok(updateResult);
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        var deleteResult = await _productRepository.DeleteProduct(id);
        return Ok(deleteResult);
    }
}
