using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Models;
using System;

namespace PharmacyOrderingWebsite.Services
{
    public class MedicineService
    {
        private readonly AppDbContext _context;

        public MedicineService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL MEDICINES WITH CORRECT STOCK
        public async Task<List<MedicineDto>> GetAll()
        {
            var medicines = await _context.Medicines.ToListAsync();
            var inventories = await _context.Inventories.ToListAsync();

            return medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Dosage = m.Dosage,
                Price = m.Price,
                CategoryId = m.CategoryId,
                Stock = inventories.FirstOrDefault(i => i.MedicineId == m.Id)?.Stock ?? 0
            }).ToList();
        }

        // ✅ GET BY ID
        public async Task<MedicineDto?> GetById(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return null;

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicineId == id);

            return new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Dosage = medicine.Dosage,
                Price = medicine.Price,
                CategoryId = medicine.CategoryId,
                Stock = inventory?.Stock ?? 0
            };
        }

        // ✅ CREATE MEDICINE + INVENTORY
        public async Task<MedicineDto> Create(MedicineDto dto)
        {
            var medicine = new Medicine
            {
                Name = dto.Name,
                Dosage = dto.Dosage,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            var inventory = new Inventory
            {
                MedicineId = medicine.Id,
                Stock = dto.Stock
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            dto.Id = medicine.Id;
            return dto;
        }

        // ✅ UPDATE MEDICINE + HANDLE INVENTORY PROPERLY
        public async Task Update(MedicineDto dto)
        {
            var medicine = await _context.Medicines.FindAsync(dto.Id);

            if (medicine != null)
            {
                medicine.Name = dto.Name;
                medicine.Dosage = dto.Dosage;
                medicine.Price = dto.Price;
                medicine.CategoryId = dto.CategoryId;

                // 🔥 FIX: HANDLE INVENTORY EVEN IF MISSING
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.MedicineId == dto.Id);

                if (inventory == null)
                {
                    inventory = new Inventory
                    {
                        MedicineId = dto.Id,
                        Stock = dto.Stock
                    };
                    _context.Inventories.Add(inventory);
                }
                else
                {
                    inventory.Stock = dto.Stock;
                }

                await _context.SaveChangesAsync();
            }
        }

        // ✅ DELETE (optional: also remove inventory)
        public async Task Delete(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine != null)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.MedicineId == id);

                if (inventory != null)
                {
                    _context.Inventories.Remove(inventory);
                }

                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
            }
        }
    }
}