using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Services;

namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionService _service;

        public PrescriptionController(PrescriptionService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                //Get userId from token safely
                var userIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    return Unauthorized("Invalid token");

                var userId = int.Parse(userIdClaim);

                // check file
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                var result = await _service.Upload(userId, file);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Upload failed");
            }
        }

        [HttpPut("validate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Validate(int id, UpdatePrescriptionStatusDto dto)
        {
            try
            {
                var result = await _service.Validate(id, dto.Status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Validation failed");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
    }
}