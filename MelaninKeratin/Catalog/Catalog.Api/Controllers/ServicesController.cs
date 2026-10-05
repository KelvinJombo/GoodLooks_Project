using Catalog.Application.Dtos.Service;
using Catalog.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiService _serviService;

        public ServicesController(IServiService serviService)
        {
            _serviService = serviService;
        }

        [HttpPost]
        public async Task<IActionResult> AddService([FromBody] CreateServiceDto serviceDto)
        {
            var response = _serviService.CreateServi(serviceDto);

            if (response == null)
                return BadRequest(response);

            return CreatedAtAction(nameof(AddService), response);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = _serviService.GetAllServi();

            if (response == null)
                return NotFound();

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = _serviService.GetServiById(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditService([FromBody] UpdateServiceDto serviceDto, string id)
        {
            var response = _serviService.UpdateServi(id, serviceDto);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveService([FromRoute] string id)
        {
            var response = _serviService.DeleteServiById(id);   

            if (response == null)
                return NotFound();

            return NoContent();
        }


    }
}
