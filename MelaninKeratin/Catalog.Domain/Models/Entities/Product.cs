using Catalog.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Models.Entities
{
    public class Product
    {
        [Key]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Brand { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public ProductType Type { get; private set; }
        public string? TypeOption { get; private set; }
        public ICollection<Photo> Photos { get; private set; }
            = new List<Photo>();
    }
}
