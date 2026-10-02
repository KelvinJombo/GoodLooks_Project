using Catalog.Application.Dtos;
using Catalog.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces.IService
{
    public interface IProductService
    {
        Task<ProductResponseDto>CreateProductDto(CreateProductDto dto);
        Task<ProductResponseDto> GetProductById(string id); 
        Task<ProductResponseDto> GetProducts();
        Task<ProductResponseDto> UpdateProductDto(UpdateProductDto dto);
        Task<bool> DeleteProductById(string id);
    }
}
