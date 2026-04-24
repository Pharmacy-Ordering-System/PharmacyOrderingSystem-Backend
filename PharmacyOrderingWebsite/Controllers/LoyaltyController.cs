using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.Models;
using PharmacyOrderingWebsite.Services;
using PharmacyOrderingWebsite.DTOs;
namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/loyalty")]
    [Authorize]
    public class LoyaltyController : ControllerBase
    {
        private readonly LoyaltyService _service;

        public LoyaltyController(LoyaltyService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(LoyaltyDto dto)
        {
            var result = await _service.AddPoints(dto.UserId, dto.Points);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int userId)
        {
            return Ok(await _service.Get(userId));
        }
    }
}