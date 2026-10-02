using Catalog.Domain.Models.Entities;
using Catalog.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Dtos.Product
{
    public class ProductResponseDto
    {
        public string Id { get; private set; }        
        public string Name { get; private set; } = string.Empty;        
        public string Brand { get; private set; } = string.Empty;
        public decimal Price { get; private set; }        
        public ICollection<Photo> Photos { get; private set; }
            = new List<Photo>();
    }
}
