using Catalog.Application.Dtos.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces.IService
{
    public interface IServiService
    {
        Task<ServiceResponseDto> CreateServi(CreateServiceDto dto);
        Task<ServiceResponseDto> GetAllServi();
        Task<ServiceResponseDto> GetServiById(string id);
        Task<ServiceResponseDto> UpdateServi(UpdateServiceDto dto);
        Task<bool> DeleteServiById(string id);

    }
}
