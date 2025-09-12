using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Repositories.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public Task DeleteAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction?> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Transaction>?> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Unit)
                .Include(t => t.UtilityBill)
                .Include(t => t.TransactionCategory)
                .Include(t => t.TransactionType)
                .Include(t => t.PaymentMethod)
                .ToListAsync();
        }


        public Task<Transaction?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> UpdateAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
