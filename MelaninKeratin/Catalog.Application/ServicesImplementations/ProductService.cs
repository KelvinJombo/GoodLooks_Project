using Catalog.Application.Dtos;
using Catalog.Application.Dtos.Product;
using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Application.Interfaces.IService;
using Catalog.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.ServicesImplementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;

        public ProductService(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }

        public Task<ProductResponseDto> CreateProductDto(CreateProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDto> GetProductById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDto> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDto> UpdateProductDto(UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
