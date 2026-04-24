using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.Models;
using System;

namespace PharmacyOrderingWebsite.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory?> GetByMedicineId(int medicineId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicineId == medicineId);
        }

        public async Task UpdateStock(int medicineId, int quantity)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicineId == medicineId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    MedicineId = medicineId,
                    Stock = quantity
                };

                _context.Inventories.Add(inventory);
            }
            else
            {
                inventory.Stock = quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ReduceStock(int medicineId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            using var transaction = await _context.Database.BeginTransactionAsync();

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicineId == medicineId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            if (inventory.Stock < quantity)
                throw new Exception("Insufficient stock");

            inventory.Stock -= quantity;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<Inventory>> GetAllInventory()
        {
            return await _context.Inventories
                .Include(i => i.Medicine)
                .ToListAsync();
        }
        public async Task<List<Inventory>> GetExpiringSoon()
        {
            var today = DateTime.UtcNow;
            var next30Days = today.AddDays(30);

            return await _context.Inventories
                .Include(i => i.Medicine)
                .Where(i => i.ExpiryDate != null && i.ExpiryDate <= next30Days)
                .ToListAsync();
        }
    }
}