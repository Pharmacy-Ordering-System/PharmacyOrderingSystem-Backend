
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Services;

namespace PharmacyOrderingWebsite.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _service.Register(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.Login(dto);
        return Ok(result);
    }
}