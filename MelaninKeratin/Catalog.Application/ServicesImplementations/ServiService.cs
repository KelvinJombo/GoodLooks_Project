using Catalog.Application.Dtos.Service;
using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Application.Interfaces.IService;
using Catalog.Domain.Models.Entities;
using Catalog.Domain.Models.Response;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.ServicesImplementations
{
    public class ServiService : IServiService
    {
        private readonly IGenericRepository<Service> _repository;

        public ServiService(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }
        public async Task<ApiResponse<ServiceResponseDto>> CreateServi(CreateServiceDto dto)
        {
            var serviMap = dto.Adapt<Service>();   
            
            if (serviMap == null)
                return ApiResponse<ServiceResponseDto>.Failed("The request body should not be null", StatusCodes.Status400BadRequest, new List<string>());

            var servi = _repository.AddAsync(serviMap);

            var mapped = servi.Adapt<ServiceResponseDto>();

            return ApiResponse<ServiceResponseDto>.Success(mapped, "The Service is created Successfully", StatusCodes.Status201Created);
        }

        public async Task<ApiResponse<bool>> DeleteServiById(string id)
        {
            var serviInDb = await _repository.GetByIdAsync(id);

            if (serviInDb == null)
                return ApiResponse<bool>.Failed("Service with the given Id was not found", StatusCodes.Status400BadRequest, new List<string>());

             _repository.DeleteAsync(serviInDb);

            return ApiResponse<bool>.Success(true, "This Service has been deleted successfully", StatusCodes.Status204NoContent);
        }

        public async Task<ApiResponse<IEnumerable<ServiceResponseDto>>> GetAllServi()
        {
            var servi = _repository.GetAllAsync();

            if (servi == null)
                return ApiResponse<IEnumerable<ServiceResponseDto>>.Failed("No Service was found to display", StatusCodes.Status400BadRequest, new List<string>());

            var mapped = servi.Adapt<List<ServiceResponseDto>>();

            return ApiResponse<IEnumerable<ServiceResponseDto>>.Success(mapped, "All RequestedServices Retieved Successfully", StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<ServiceResponseDto>> GetServiById(string id)
        {
            var servi = await _repository.GetByIdAsync(id);

            if (servi == null)
                return ApiResponse<ServiceResponseDto>.Failed("Service with the given Id was not found", StatusCodes.Status404NotFound, new List<string>());

            var mapped = servi.Adapt<ServiceResponseDto>();

            return ApiResponse<ServiceResponseDto>.Success(mapped, "Serve was Successfully Retrieved", StatusCodes.Status200OK);


        }

        public async Task<ApiResponse<ServiceResponseDto>> UpdateServi(string id, UpdateServiceDto dto)
        {
            var servi = _repository.GetByIdAsync(id);

            if (servi == null)
                return ApiResponse<ServiceResponseDto>.Failed("The Service with the given If ford not exist", StatusCodes.Status400BadRequest, new List<string>());

            var map = servi.Adapt<ServiceResponseDto>();

            return ApiResponse<ServiceResponseDto>.Success(map, "The Service was updated Successfully", StatusCodes.Status200OK);
        }
    }
}
