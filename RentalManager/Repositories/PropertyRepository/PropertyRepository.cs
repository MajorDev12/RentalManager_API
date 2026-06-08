using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;
using System.Linq.Expressions;

namespace RentalManager.Repositories.PropertyRepository
{
    public class PropertyRepository : IPropertyRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public PropertyRepository(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }



        public async Task<Property> AddProperty(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }


        public async Task<Property?> GetByIdAsync(int id)
        {
            return await _context.Properties
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .Include(u => u.PropertyType)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Property>?> GetAllAsync()
        {
            return await _context.Properties
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .Include(u => u.PropertyType)
                .ToListAsync();
        }

        public async Task<List<READPropertyLookupDto>> GetAllLookupAsync()
        {
            return await _context.Properties
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .Select(p => new READPropertyLookupDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }


        public async Task<(List<Property>, int)> GetFilteredAsync(PropertyQueryFilter filter)
        {
            var query = _context.Properties
                .Include(x => x.PropertyType)
                .AsNoTracking()
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .AsQueryable();

            query = query
                .ApplyPropertySearch(filter)
                .ApplyPropertyFilters(filter)
                .ApplyPropertySorting(filter);

            var totalRecords = await query.CountAsync();

            var data = await query
                .ApplyPagination(filter.PageNumber, filter.PageSize)
                .ToListAsync();

            return (data, totalRecords);
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

        public async Task<Property?> FindAsync(int id)
        {
            return await _context.Properties
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Properties
                .ApplyRoleFilter(_currentUser, _context, p => p.Id)
                .AnyAsync(u => u.Id == id);
        }
    }
}
