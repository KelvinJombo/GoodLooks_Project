using Catalog.Application.Dtos.Cloudinary;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces.IService
{
    public interface ICloudinaryServices<T> where T : class
    {
        Task<CloudinaryUploadResponse> UploadImageAsync(IFormFile file, string folder);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
