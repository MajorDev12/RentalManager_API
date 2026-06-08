using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.TenantRepository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentuser;

        public TenantRepository(ApplicationDbContext context, ICurrentUser currentuser)
        {
            _context = context;
            _currentuser = currentuser;
        }


        public async Task<List<Tenant>?> GetAllAsync()
        {
            return await _context.Tenants
                .ApplyRoleFilter(_currentuser, _context)
                .WithDetails()
                .ToListAsync();
        }


        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentuser, _context)
                .WithDetails()
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetUserIdByTenantIdAsync(int tenantId)
        {
            return await _context.Tenants
                .Where(u => u.Id == tenantId)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<Tenant?> GetByUserIdAsync(int userId)
        {
            return await _context.Tenants
                .Where(u => u.UserId ==  userId)
                .ApplyRoleFilter(_currentuser, _context)
                .Include(t => t.Unit)
                .FirstOrDefaultAsync(pr => pr.UserId == userId);
        }


        public async Task<List<Tenant>?> GetAllByPropertyId(int propertyId, bool? isActive)
        {
            var query = _context.Tenants
                        .ByProperty(propertyId)
                        .ApplyRoleFilter(_currentuser, _context);

            if (isActive == true)
                query = query.IsActive();

            return await query
                .WithDetails()
                .ToListAsync();
        }


        public async Task<List<Tenant>?> GetAllByUnitId(int unitId)
        {
            var query = _context.Tenants
                        .ByUnit(unitId)
                        .ApplyRoleFilter(_currentuser, _context);


            return await query
                .WithDetails()
                .ToListAsync();
        }


        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }


        public async Task<int> UpdateAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Tenant tenant)
        {
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
        }


        public async Task<Tenant?> FindAsync(int id)
        {
            return await _context.Tenants
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentuser, _context)
                .FirstOrDefaultAsync();
        }


        public async Task<Tenant?> AssignUnitAsync(Tenant tenant)
        {
            var existingTenant = await FindAsync(tenant.Id);

            if (existingTenant == null) return null;

            var updatedEntity = tenant.UpdateEntity(existingTenant);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task<Tenant?> UpdateTenantStatusAsync(int tenantId, int tenantStatusId)
        {
            var tenant = await _context.Tenants
                .Include(t => t.TenantStatus)
                .FirstOrDefaultAsync(t => t.Id == tenantId);

            if (tenant == null)
                return null;

            tenant.TenantStatusId = tenantStatusId;
            // if status is not active
            if(tenant.TenantStatus.Item != "Active")
            {
                tenant.UnitId = null;
            }

            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }

    }
}
