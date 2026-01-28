using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Helpers.Authorization;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;
using System.Security.Claims;

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


        public async Task<Property?> GetByIdAsync(ICurrentUser user, int id)
        {
            return await _context.Properties
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Property>?> GetAllAsync(ICurrentUser user)
        {
            return await _context.Properties
                .ApplyRoleFilter(user, _context)
                .ToListAsync();
        }


        public async Task UpdateAsync(Property property)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Property property)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }

        public async Task<Property?> FindAsync(ICurrentUser user, int id)
        {
            return await _context.Properties
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .FirstOrDefaultAsync();
        }
    }
}
