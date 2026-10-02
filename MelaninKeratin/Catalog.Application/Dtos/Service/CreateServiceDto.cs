using Catalog.Domain.Models.Entities;
using Catalog.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Dtos.Service
{
    public class CreateServiceDto
    {
        
        public string StyleName { get; private set; } = string.Empty;
        public decimal PriceTag { get; private set; }
        public ServiceType Type { get; private set; }
        public string? TypeOption { get; set; }
        public ICollection<Photo> Photos { get; private set; }
            = new List<Photo>();
    }
}
