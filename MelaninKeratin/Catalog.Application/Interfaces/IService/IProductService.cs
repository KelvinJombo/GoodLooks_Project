using Catalog.Application.Dtos.Product;
using Catalog.Domain.Models.Response;

namespace Catalog.Application.Interfaces.IService
{
    public interface IProductService
    {
       
        Task<ApiResponse<ProductResponseDto>> CreateProduct(CreateProductDto dto);
        Task<ApiResponse<ProductResponseDto>> GetProductById(string id);
        Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetProducts();
        Task<ApiResponse<ProductResponseDto>> UpdateProduct(string id, UpdateProductDto dto);
        Task<ApiResponse<bool>> DeleteProductById(string id);
    }
}
