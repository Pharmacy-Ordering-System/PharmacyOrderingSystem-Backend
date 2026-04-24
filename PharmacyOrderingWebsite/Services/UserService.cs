using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.DTOs;

namespace PharmacyOrderingWebsite.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 Get all users (Admin)
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "User")
                .Select(u => new UserDto
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        // 🔹 Get user by ID
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            return new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        // 🔹 Update user
        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            user.Name = dto.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Delete user (Admin)
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<object> GetUserFullDetails(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,

                    Orders = _context.Orders
                        .Where(o => o.UserId == userId)
                        .Select(o => new
                        {
                            o.Id,
                            o.TotalAmount,
                            o.Status,
                            o.CreatedAt,
                            o.Items
                        }).ToList(),

                    Prescriptions = _context.Prescriptions
                        .Where(p => p.UserId == userId)
                        .Select(p => new
                        {
                            p.Id,
                            p.FilePath,
                            p.Status
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return user!;
        }
    }
}