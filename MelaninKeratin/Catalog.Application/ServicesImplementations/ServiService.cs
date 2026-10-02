using Catalog.Application.Dtos.Service;
using Catalog.Application.Interfaces.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.ServicesImplementations
{
    public class ServiService : IServiService
    {
        public Task<ServiceResponseDto> CreateServi(CreateServiceDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteServiById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseDto> GetAllServi()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseDto> GetServiById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseDto> UpdateServi(UpdateServiceDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
