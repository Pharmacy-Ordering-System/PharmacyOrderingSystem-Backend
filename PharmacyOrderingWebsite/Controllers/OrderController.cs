using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Services;

namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(OrderDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User not found in token");

                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Invalid user id in token");

                var result = await _service.CreateOrder(dto, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            try
            {
                var updatedOrder = await _service.UpdateOrderStatus(id, dto.Status);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAll();
            return Ok(orders);
        }
    }
}