
using System.ComponentModel.DataAnnotations;

namespace PharmacyOrderingWebsite.DTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}