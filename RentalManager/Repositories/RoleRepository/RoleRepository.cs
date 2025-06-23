using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Role;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role> AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }


        public async Task<List<Role>> GetAllAsync()
        {
            var roles = await _context.Roles
                .Include(ub => ub.Property)
                .ToListAsync();

            return roles;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(c => c.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            return role;
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existingRole = await FindAsync(role.Id);

            if (existingRole == null)
                return null;

            var updated = role.ToUpdateEntity(existingRole);
            await _context.SaveChangesAsync();

            return updated;
        }

        public async Task<Role> DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> FindAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            return role;
        }
    }
}
