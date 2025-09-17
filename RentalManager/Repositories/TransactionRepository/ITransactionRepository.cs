using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.Repositories.TransactionRepository
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>?> GetAllAsync();

        Task<Transaction?> GetByIdAsync(int id);

        Task<Transaction> AddAsync(Transaction transaction);

        Task<int> UpdateAsync();

        Task DeleteAsync(Transaction transaction);

        Task<Transaction?> FindAsync(int id);

        Task<List<TenantBalanceDto>> GetBalancesAsync();
    }
}
