using Domain.ApiResponse;
using Domain.Entities;
using Domain.Filters;
using Domain.Paginations;
using Infrastructure.Data;
using Infrastructure.Products.ProductInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Products;

public class ProductRepositry(DataContext context, ILogger<ProductRepositry> logger) : IProductRepository
{

    public async Task<Product?> GetProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            logger.LogError($"Product with id-{id} not found");
        }
        return product;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<int> AddProductAsync(Product product)
    {
        try
        {
            await context.Products.AddAsync(product);
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return 0;
        }
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        try
        {
            context.Products.Update(product);
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return 0;
        }
    }

    public async Task<int> DeleteProductAsync(Product product)
    {
        try
        {
            context.Products.Remove(product);
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return 0;
        }
    }
}
