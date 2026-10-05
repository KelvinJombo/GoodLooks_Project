using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Dtos.Service
{
    public class UpdateServiceDto
    {
        public string StyleName { get; private set; } = string.Empty;
        public string Description { get; private set; }
        public decimal PriceTag { get; private set; }       
        public string? TypeOption { get; set; }
        public IFormFile? Image { get; set; }        
        public bool IsMainPhoto { get; set; } = true;
    }
}
