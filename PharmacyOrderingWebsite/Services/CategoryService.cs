using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Models;
using System;

namespace PharmacyOrderingWebsite.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<CategoryDto?> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> Create(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            dto.Id = category.Id;
            return dto;
        }

        public async Task Update(CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.Id);

            if (category != null)
            {
                category.Name = dto.Name;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}