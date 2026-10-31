using Catalog.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Models.Entities
{
    public class Service
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PriceTag { get; set; }  
        public ServiceType Type { get; set; }
        public string? TypeOption { get; set; }
        public ICollection<Photo> Photos { get; set; }
            = new List<Photo>();
    }
}
