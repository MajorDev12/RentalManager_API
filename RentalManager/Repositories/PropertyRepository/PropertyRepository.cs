using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.PropertyRepository
{
    public class PropertyRepository : IPropertyRepository
    {

        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<Property> AddProperty(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<Property> GetByIdAsync(int id)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(pr => pr.Id == id);
            return property;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            var properties = await _context.Properties.ToListAsync();
            return properties;
        }

        public async Task<Property> UpdateAsync(Property property)
        {
            var existing = await FindAsync(property.Id);
            if (existing == null) return null;

            var updated = existing.ToUpdateEntity(property);

            await _context.SaveChangesAsync();
            return existing;

        }

        public async Task DeleteAsync(Property property)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }

        public async Task<Property> FindAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return null;
            return property;
        }
    }
}
