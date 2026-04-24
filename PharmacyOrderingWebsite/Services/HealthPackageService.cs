using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Models;
using System;
namespace PharmacyOrderingWebsite.Services
{
    public class HealthPackageService
    {
        private readonly AppDbContext _context;

        public HealthPackageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthPackage>> GetAll()
        {
            return await _context.HealthPackages.ToListAsync();
        }

        public async Task<HealthPackage> GetById(int id)
        {
            return await _context.HealthPackages.FindAsync(id);
        }

        public async Task<HealthPackage> Create(HealthPackageDto dto)
        {
            var package = new HealthPackage
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            _context.HealthPackages.Add(package);
            await _context.SaveChangesAsync();

            return package;
        }

        public async Task<HealthPackage> Update(int id, HealthPackageDto dto)
        {
            var package = await _context.HealthPackages.FindAsync(id);
            if (package == null) throw new Exception("Not found");

            package.Name = dto.Name;
            package.Description = dto.Description;
            package.Price = dto.Price;

            await _context.SaveChangesAsync();

            return package;
        }

        public async Task<bool> Delete(int id)
        {
            var package = await _context.HealthPackages.FindAsync(id);
            if (package == null) return false;

            _context.HealthPackages.Remove(package);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}