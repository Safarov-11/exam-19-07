using Domain.ApiResponse;
using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Products.ProductInterfaces;

public interface IProductService
{
    Task<Response<Product?>> GetProductAsync(int id);
    Task<PagedResponse<List<Product>>> GetProductsAsync(ProductFilter filter);
    Task<Response<string>> AddProductAsync(Product product);
    Task<Response<string>> UpdateProductAsync(Product product);
    Task<Response<string>> DeleteProductAsync(int id);

}