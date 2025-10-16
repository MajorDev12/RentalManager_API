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


        public async Task<List<Role>?> GetAllAsync()
        {
            var roles = await _context.Roles
                .ToListAsync();

            return roles;
        }


        public async Task<Role?> GetByIdAsync(int id)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(pr => pr.Id == id);

            return role;
        }


        public async Task<Role?> GetByNameAsync(string name)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(pr => pr.Name == name);

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


        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }


        public async Task<Role> FindAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
    }
}
