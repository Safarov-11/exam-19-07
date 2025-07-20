using System.Net;
using Domain.ApiResponse;
using Domain.Entities;
using Domain.Filters;
using Domain.Paginations;
using Infrastructure.Interfaces;
using Infrastructure.Products.ProductInterfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Products;

public class ProductService(
    IProductRepository repository,
    IRedisCacheService redisCache,
    ILogger<ProductService> logger) : IProductService
{
    public async Task<Response<Product?>> GetProductAsync(int id)
    {
        var product = await repository.GetProductAsync(id);
        if (product == null)
        {
            return Response<Product?>.Error("Product not found", HttpStatusCode.NotFound);
        }

        return Response<Product?>.Success(product);
    }

    public async Task<PagedResponse<List<Product>>> GetProductsAsync(ProductFilter filter)
    {
        var validfilter = new ValidFilter(filter.PageSize, filter.PageNumber);
        const string key = "products";
        var productCache = await redisCache.GetData<List<Product>>(key);
        if (productCache == null)
        {
            productCache = await repository.GetProductsAsync();
            await redisCache.SetData(key, productCache, 5);
            logger.LogInformation("Cache created");
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            productCache = productCache
                .Where(p => p.Name.ToLower().Trim()
                .Contains(filter.Name.ToLower().Trim())).ToList();
        }

        if (filter.StartPrice.HasValue)
        {
            productCache = productCache.Where(p => p.Price >= filter.StartPrice).ToList();
        }

        if (filter.ToPrice.HasValue)
        {
            productCache = productCache.Where(p => p.Price <= filter.ToPrice).ToList();
        }

        var totalCount = productCache.Count();
        var pagination = productCache
        .Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
        .Take(validfilter.PageSize)
        .ToList();

        return new PagedResponse<List<Product>>(pagination, validfilter.PageSize, validfilter.PageNumber, totalCount);
    }

    public async Task<Response<string>> AddProductAsync(Product product)
    {
        var result = await repository.AddProductAsync(product);
        if (result != 1)
        {
            return Response<string>.Error("Failed to add product", HttpStatusCode.InternalServerError);
        }
        await redisCache.RemoveData("products");
        logger.LogInformation("old cache removed");

        return Response<string>.Success(null, "Product created!");
    }

    public async Task<Response<string>> DeleteProductAsync(int id)
    {
        var product = await repository.GetProductAsync(id);
        if (product == null)
        {
            return Response<string>.Error("Product not found", HttpStatusCode.NotFound);
        }

        var result = await repository.DeleteProductAsync(product);
        if (result != 1)
        {
            return Response<string>.Error("Failed to delete product", HttpStatusCode.InternalServerError);
        }

        await redisCache.RemoveData("products");
        logger.LogInformation("old cache removed");
        return Response<string>.Success(null, "Product deleted!");

    }

    public async Task<Response<string>> UpdateProductAsync(Product updProduct)
    {
        var product = await repository.GetProductAsync(updProduct.Id);
        if (product == null)
        {
            return Response<string>.Error("Product not found", HttpStatusCode.NotFound);
        }

        product.Name = updProduct.Name;
        product.Price = updProduct.Price;
        product.CreatedAt = updProduct.CreatedAt;

        var result = await repository.UpdateProductAsync(product);
        if (result != 1)
        {
            return Response<string>.Error("Failed to update product", HttpStatusCode.InternalServerError);
        }

        await redisCache.RemoveData("products");
        logger.LogInformation("old cache removed");
        return Response<string>.Success(null, "Product updated!");
    }
}
