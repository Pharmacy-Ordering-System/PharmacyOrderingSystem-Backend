using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Helpers;
using PharmacyOrderingWebsite.Models;


namespace PharmacyOrderingWebsite.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwt;
        private readonly PasswordHasher _hasher;
        private readonly EmailService _emailService;

        public AuthService(
            AppDbContext context,
            JwtHelper jwt,
            PasswordHasher hasher,
            EmailService emailService)
        {
            _context = context;
            _jwt = jwt;
            _hasher = hasher;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            Console.WriteLine("[AUTH SERVICE] Register started");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                Console.WriteLine("[AUTH SERVICE] User already exists");
                throw new Exception("User already exists");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _hasher.Hash(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine("[AUTH SERVICE] User registered successfully");

            try
            {
                var subject = "Welcome to Pharmacy Ordering System";
                var body = $@"
                    <h2>Hello {user.Name},</h2>
                    <p>Your account has been created successfully.</p>
                    <p><b>Email:</b> {user.Email}</p>
                    <p>You can now login and use the Pharmacy Ordering System.</p>
                    <br/>
                    <p>Regards,<br/>Pharmacy Ordering Team</p>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
                Console.WriteLine("[AUTH SERVICE] Registration email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUTH SERVICE] Registration email failed: {ex.Message}");
            }

            return new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user),
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            Console.WriteLine("[AUTH SERVICE] Login started");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
            {
                Console.WriteLine("[AUTH SERVICE] Invalid credentials");
                throw new Exception("Invalid credentials");
            }

            Console.WriteLine("[AUTH SERVICE] Login successful");

            try
            {
                var subject = "Login Alert - Pharmacy Ordering System";
                var body = $@"
                    <h2>Hello {user.Name},</h2>
                    <p>Your account was logged in successfully.</p>
                    <p><b>Email:</b> {user.Email}</p>
                    <p>If this was not you, please reset your password immediately.</p>
                    <br/>
                    <p>Regards,<br/>Pharmacy Ordering Team</p>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
                Console.WriteLine("[AUTH SERVICE] Login email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUTH SERVICE] Login email failed: {ex.Message}");
            }

            return new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user),
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}