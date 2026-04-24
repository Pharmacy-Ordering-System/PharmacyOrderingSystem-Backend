using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmacyOrderingWebsite.Services;

namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{medicineId}")]
        public async Task<IActionResult> GetStock(int medicineId)
        {
            return Ok(await _service.GetByMedicineId(medicineId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{medicineId}")]
        public async Task<IActionResult> UpdateStock(int medicineId, [FromQuery] int quantity)
        {
            await _service.UpdateStock(medicineId, quantity);
            return Ok();
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInventory()
        {
            var data = await _service.GetAllInventory();
            return Ok(data);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiring()
        {
            var data = await _service.GetExpiringSoon();
            return Ok(data);
        }
    }
}