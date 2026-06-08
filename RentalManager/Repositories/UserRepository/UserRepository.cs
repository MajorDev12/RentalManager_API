using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAccountContext _accountcontext;
        public UserRepository(
            ApplicationDbContext context,
            IAccountContext accountcontext,
            UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _accountcontext = accountcontext;
            _usermanager = usermanager;
        }


        public async Task<List<User>?> GetAllAsync()
        {
            return await _context.Users
                .Include(ub => ub.Property)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Gender)
                .ToListAsync();
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(c => c.Property)
                .Include(ub => ub.UserStatus)
                .Include(ub => ub.Gender)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }



        public async Task<ApplicationUser?> GetByApplicationUserIdAsync(int id)
        {
            return await _usermanager.Users
                .Include(u => u.User)
                    .ThenInclude(u => u.Gender)
                .Include(u => u.User)
                    .ThenInclude(u => u.UserStatus)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetIdByApplicationUserIdAsync(int appUserId)
        {
            return await _context.Users
                    .Where(u => u.ApplicationUserId == appUserId)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();
        }


        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<User> UpdateAsync(User user)
        {
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<ApplicationUser?> GetByNumberAsync(string number)
        {

            return await _context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == number);
        }



        public async Task<User?> FindAsync(int id)
        {
            return await _context.Users
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser?> FindByAppUserIdAsync(int id)
        {
            return await _usermanager.Users
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

    }
}
