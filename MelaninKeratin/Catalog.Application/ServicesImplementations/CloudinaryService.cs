using Catalog.Application.Dtos.Cloudinary;
using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Application.Interfaces.IService;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MiHairCareApp.Domain.Entities.Helper;

namespace Catalog.Application.ServicesImplementations
{
    public class CloudinaryServices<TEntity> : ICloudinaryServices<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices(IGenericRepository<TEntity> repository, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            var cloudinarySettings = new CloudinarySettings();
            configuration.GetSection("CloudinarySettings").Bind(cloudinarySettings);

            _cloudinary = new Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret));
        }

        public async Task<CloudinaryUploadResponse> UploadImageAsync(IFormFile file, string folder)
        {

            if (file == null || file.Length == 0)
                return null;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            var response = new CloudinaryUploadResponse
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri.ToString(),
            };
            return response;
        }



        public async Task<DeletionResult> DeletePhotoAsync(string Id)
        {
            var DeleteParams = new DeletionParams(Id);
            var result = await _cloudinary.DestroyAsync(DeleteParams);

            return result;
        }



    }
}
