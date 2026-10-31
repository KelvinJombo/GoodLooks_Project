using Catalog.Application.Dtos.Product;
using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Application.Interfaces.IService;
using Catalog.Domain.Models.Entities;
using Catalog.Domain.Models.Response;
using Mapster;
using Microsoft.AspNetCore.Http;
using MiHairCareApp.Domain.Constants;

namespace Catalog.Application.ServicesImplementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly ICloudinaryServices<ProductService> _cloudinaryServices;

        public ProductService(IGenericRepository<Product> repository, ICloudinaryServices<ProductService> cloudinaryServices)
        {
            _repository = repository;
            _cloudinaryServices = cloudinaryServices;
        }

        public async Task<ApiResponse<ProductResponseDto>> CreateProduct(CreateProductDto dto)
        {
           var productModel = dto.Adapt<Product>();

            if (dto.Image != null)
            {
                var img = await _cloudinaryServices.UploadImageAsync(dto.Image, $"{"mihaircare"}/{CloudinaryFolders.ProductImages}/{productModel.Id}");
                if (img == null)
                   return ApiResponse<ProductResponseDto>.Failed("Image upload failed", StatusCodes.Status500InternalServerError, new List<string>());

                var photo = new Photo { Url = img.Url, PublicId = img.PublicId, IsMainPhoto = dto.IsMainPhoto };
                productModel.Photos = new List<Photo> { photo };
            }

            var saved = _repository.AddAsync(productModel);


            var productDto = saved.Adapt<ProductResponseDto>();

            return ApiResponse<ProductResponseDto>.Success(productDto, "Product Added Successfully", StatusCodes.Status201Created);
        }

        public async Task<ApiResponse<bool>> DeleteProductById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ApiResponse<bool>.Failed("The ProductId must not be null, please behave", StatusCodes.Status400BadRequest, new List<string>());

            var product = await _repository.GetByIdAsync(id);

            if (product != null)
                _repository.DeleteAsync(product);

            return ApiResponse<bool>.Success(true, "Product Deleted Successfully", StatusCodes.Status204NoContent);
        }

        public async Task<ApiResponse<ProductResponseDto>> GetProductById(string id)
        {
           var product = _repository.GetByIdAsync(id);

            if (product == null)
                ApiResponse<ProductResponseDto>.Failed("Product does not exist", StatusCodes.Status404NotFound, new List<string>());

               var productDto = product.Adapt<ProductResponseDto>();

            return ApiResponse<ProductResponseDto>.Success(productDto, "Product Retrieved Successfully", StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = _repository.GetAllAsync();

            if (products == null)
                return ApiResponse<IEnumerable<ProductResponseDto>>.Failed("Expected Products not found", StatusCodes.Status404NotFound, new List<string>());

            var productDtos = products.Adapt<List<ProductResponseDto>>();

            return ApiResponse<IEnumerable<ProductResponseDto>>.Success(productDtos, "Available Products Retrieved Successfully", StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<ProductResponseDto>> UpdateProduct(string id, UpdateProductDto dto)
        {
            var productInDb = _repository.GetByIdAsync(id);

            if (productInDb == null)
                return ApiResponse<ProductResponseDto>.Failed("Product with given Id does not exist", StatusCodes.Status404NotFound, new List<string>());

            var mapped = dto.Adapt<Product>();

            if (dto.Image != null)
            {
                var img = await _cloudinaryServices.UploadImageAsync(dto.Image, $"{"mihaircare"}/{CloudinaryFolders.ProductImages}/{mapped.Id}");
                if (img == null)
                    return ApiResponse<ProductResponseDto>.Failed("Image upload failed", StatusCodes.Status500InternalServerError, new List<string>());

                var photo = new Photo { Url = img.Url, PublicId = img.PublicId, IsMainPhoto = dto.IsMainPhoto };
                mapped.Photos = new List<Photo> { photo };
            }

            var productResponseDto = mapped.Adapt<ProductResponseDto>();

            _repository.Update(mapped);

            return ApiResponse<ProductResponseDto>.Success(productResponseDto, "Product Updated Successfully", StatusCodes.Status200OK);
        }
    }
}
