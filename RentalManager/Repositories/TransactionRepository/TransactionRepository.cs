using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Transaction;
using RentalManager.DTOs.User;
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


        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }


        public async Task<Transaction?> FindAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }


        public async Task<List<Transaction>?> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .Include(p => p.Property)
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


        public async Task<int> UpdateAsync()
        {
            var changes = await _context.SaveChangesAsync();

            return changes;
        }


        public async Task<List<TenantBalanceDto>> GetBalancesAsync()
        {
            return await _context.Transactions
                .Where(t => t.UserId != null && t.TransactionCategory.Item == "Rent")
                .GroupBy(t => new { t.UserId, t.MonthFor, t.YearFor })
                .Select(g => new TenantBalanceDto
                {
                    UserId = g.Key.UserId.Value,

                    // ✅ project scalar properties instead of returning nav objects
                    FullName = g.Select(x => x.User.FirstName + " " + x.User.LastName).FirstOrDefault() ?? "",
                    UnitName = g.Select(x => x.Unit.Name).FirstOrDefault() ?? "",
                    PropertyName = g.Select(x => x.Unit.Property.Name).FirstOrDefault() ?? "",

                    Month = g.Key.MonthFor,
                    Year = g.Key.YearFor,

                    TotalCharges = g.Where(t => t.TransactionType.Item == "Charge").Sum(t => t.Amount),
                    TotalPayments = g.Where(t => t.TransactionType.Item == "Payment").Sum(t => t.Amount),
                    Balance = g.Where(t => t.TransactionType.Item == "Charge").Sum(t => t.Amount)
                             - g.Where(t => t.TransactionType.Item == "Payment").Sum(t => t.Amount)
                })
                .Where(b => b.Balance > 0)
                .OrderBy(b => b.Year).ThenBy(b => b.Month)
                .ToListAsync();


        }



    }
}
