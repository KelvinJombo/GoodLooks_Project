using Catalog.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Models.Entities
{
    public class Service
    {
        [Key]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; private set; } = string.Empty;
        public decimal PriceTag { get; private set; }  
        public ServiceType Type { get; private set; }
        public string? TypeOption { get; set; }
        public ICollection<Photo> Photos { get; private set; }
            = new List<Photo>();
    }
}
