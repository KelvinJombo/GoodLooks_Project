using Catalog.Application.Dtos.Service;
using Catalog.Domain.Models.Response;

namespace Catalog.Application.Interfaces.IService
{
    public interface IServiService
    {
        Task<ApiResponse<ServiceResponseDto>> CreateServi(CreateServiceDto dto);
        Task<ApiResponse<IEnumerable<ServiceResponseDto>>> GetAllServi();
        Task<ApiResponse<ServiceResponseDto>> GetServiById(string id);
        Task<ApiResponse<ServiceResponseDto>> UpdateServi(string id, UpdateServiceDto dto);
        Task<ApiResponse<bool>> DeleteServiById(string id);

    }
}
