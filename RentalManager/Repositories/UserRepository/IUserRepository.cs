using RentalManager.Models;

namespace RentalManager.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>?> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<int> GetIdByApplicationUserIdAsync(int appUserId);
        Task<ApplicationUser?> GetByApplicationUserIdAsync(int id);
        Task<ApplicationUser?> GetByNumberAsync(string number);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> FindAsync(int id);
        Task<ApplicationUser?> FindByAppUserIdAsync(int id);
    }
}
