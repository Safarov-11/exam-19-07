using Domain.ApiResponse;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Products.ProductInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Authorize (Roles = "Admin,Manager")]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpGet]
    public Task<PagedResponse<List<Product>>> GetProductsAsync([FromQuery] ProductFilter filter)
    {
        return service.GetProductsAsync(filter);
    }

    [HttpGet("{id}")]
    public async Task<Response<Product?>> GetProductAsync(int id)
    {
        return await service.GetProductAsync(id);
    }

    [HttpPost]
    public async Task<Response<string>> AddProductAsync(Product product)
    {
        return await service.AddProductAsync(product);
    }

    [HttpPut]
    public Task<Response<string>> UpdateProductAsync(Product product)
    {
        return service.UpdateProductAsync(product);
    }

    [HttpDelete("{id}")]
    public Task<Response<string>> DeleteProductAsync(int id)
    {
        return service.DeleteProductAsync(id);
    }
}
