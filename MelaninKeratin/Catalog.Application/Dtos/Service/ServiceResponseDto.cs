using Catalog.Domain.Models.Entities;

namespace Catalog.Application.Dtos.Service
{
    public class ServiceResponseDto
    {
        public int Id { get; private set; }
        public string StyleName { get; private set; } = string.Empty;
        public decimal PriceTag { get; private set; }        
        public ICollection<Photo> Photos { get; private set; }
            = new List<Photo>();
    }
}
