using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.Models;
using PharmacyOrderingWebsite.Services;
using PharmacyOrderingWebsite.DTOs;
namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/packages")]
    [Authorize]
    public class HealthPackageController : ControllerBase
    {
        private readonly HealthPackageService _service;

        public HealthPackageController(HealthPackageService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(HealthPackageDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, HealthPackageDto dto)
        {
            return Ok(await _service.Update(id, dto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}