using Domain.ApiResponse;
using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Products.ProductInterfaces;

public interface IProductRepository
{
    Task<Product?> GetProductAsync(int id);
    Task<List<Product>> GetProductsAsync();
    Task<int> AddProductAsync(Product product);
    Task<int> UpdateProductAsync(Product product);
    Task<int> DeleteProductAsync(Product product);
}