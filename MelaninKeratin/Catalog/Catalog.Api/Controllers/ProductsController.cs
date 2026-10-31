using Catalog.Application.Dtos.Product;
using Catalog.Application.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _productService.GetProducts();

            if (response  == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById([FromRoute]string id)
        {
            var response = await _productService.GetProductById(id);

            if (response == null)
                return NotFound();  

            return Ok(response); 
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]CreateProductDto productDto)
        {
            var response = await _productService.CreateProduct(productDto);

            if (response == null)
                return BadRequest(response);

            return CreatedAtAction(nameof(AddProduct), response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditProduct(string id, [FromBody]UpdateProductDto productDto)
        {
            var response = await _productService.UpdateProduct(id, productDto);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var removed = await _productService.DeleteProductById(id);

            return NoContent();
        }

    }
}