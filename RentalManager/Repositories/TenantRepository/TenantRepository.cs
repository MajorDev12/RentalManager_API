using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.TenantRepository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Tenant>?> GetAllAsync()
        {
            return await _context.Tenants
                .Include(t => t.User).ThenInclude(u => u.Role)
                .Include(t => t.User).ThenInclude(u => u.Gender)
                .Include(t => t.User).ThenInclude(u => u.UserStatus)
                .Include(t => t.User).ThenInclude(u => u.Property)
                .Include(t => t.Unit)
                .Include(t => t.TenantStatus)
                .ToListAsync();
        }


        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants
                .Include(t => t.User).ThenInclude(u => u.Role)
                .Include(t => t.User).ThenInclude(u => u.Gender)
                .Include(t => t.User).ThenInclude(u => u.UserStatus)
                .Include(t => t.User).ThenInclude(u => u.Property)
                .Include(t => t.Unit)
                .Include(t => t.TenantStatus)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }


        public async Task<Tenant?> GetByUserIdAsync(int userId)
        {
            return await _context.Tenants
                .Include(t => t.Unit)
                .FirstOrDefaultAsync(pr => pr.UserId == userId);
        }


        public async Task<List<Tenant>?> GetAllByPropertyId(int propertyId, bool? isActive)
        {
            var query = _context.Tenants
                        .Where(t => t.User.PropertyId == propertyId);

            if (isActive == true)
                query = query.Where(t => t.TenantStatus.Item.ToLower() == "active");

            return await query
                .Include(t => t.Unit)
                .Include(t => t.User)
                .ToListAsync();
        }


        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }


        public async Task<Tenant> UpdateAsync(Tenant tenant, User user)
        {
            var existingTenant= await FindAsync(tenant.Id);

            if (existingTenant == null) return null;

            var updatedEntity = tenant.UpdateEntity(tenant, user);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task DeleteAsync(Tenant tenant)
        {
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
        }


        public async Task<Tenant?> FindAsync(int id)
        {
            return await _context.Tenants.FindAsync(id);
        }


        public async Task<Tenant?> AssignUnitAsync(Tenant tenant)
        {
            var existingTenant = await FindAsync(tenant.Id);

            if (existingTenant == null) return null;

            var updatedEntity = tenant.UpdateEntity(existingTenant);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }

    }
}
