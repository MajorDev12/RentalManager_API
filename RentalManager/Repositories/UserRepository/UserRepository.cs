using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<User>?> GetAllAsync()
        {
            return await _context.Users
                .Include(ub => ub.Role)
                .Include(ub => ub.Property)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Gender)
                .ToListAsync();
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(ub => ub.Role)
                .Include(c => c.Property)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Gender)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }


        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await FindAsync(user.Id);

            if (existingUser == null) return null;

            var updatedEntity = user.UpdateEntity(existingUser);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<User?> FindAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

    }
}
