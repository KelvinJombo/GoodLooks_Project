using Catalog.Domain.Models.Entities;
using Catalog.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Dtos.Product
{
    public class UpdateProductDto
    {       
        
        public string Description { get; private set; } = string.Empty;       
        public decimal Price { get; private set; }        
        public string? TypeOption { get; private set; }
        public IFormFile? Image { get; set; }        
        public bool IsMainPhoto { get; set; } = true;
    }
}
