using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Models;
using System;
namespace PharmacyOrderingWebsite.Services
{
    public class LoyaltyService
    {
        private readonly AppDbContext _context;

        public LoyaltyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoyaltyPoints> AddPoints(int userId, int points)
        {
            try
            {
                var loyalty = await _context.LoyaltyPoints
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (loyalty == null)
                {
                    loyalty = new LoyaltyPoints
                    {
                        UserId = userId,
                        Points = points
                    };
                    _context.LoyaltyPoints.Add(loyalty);
                }
                else
                {
                    loyalty.Points += points;
                }

                await _context.SaveChangesAsync();

                Console.WriteLine("[LOYALTY] Points updated");

                return loyalty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOYALTY ERROR] {ex.Message}");
                throw;
            }
        }

        public async Task<LoyaltyPoints> Get(int userId)
        {
            return await _context.LoyaltyPoints
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}