using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Transaction;
using RentalManager.DTOs.User;
using RentalManager.Extensions.Query;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;
using System.Linq;

namespace RentalManager.Repositories.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentuser;

        public TransactionRepository(ApplicationDbContext context, ICurrentUser currentuser)
        {
            _context = context;
            _currentuser = currentuser;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }


        public async Task<int> AddRangeAsync(IEnumerable<Transaction> transactions)
        {
            await _context.Transactions.AddRangeAsync(transactions);
            var result = await _context.SaveChangesAsync();
            return result;
        }


        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }



        public async Task<Transaction?> FindAsync(int id)
        {
            return await _context.Transactions
                .ApplyRoleFilter(_currentuser, _context)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }



        public async Task<List<Transaction>?> FindByMonthAsync(int month, int year, int? propertyId, int? categoryId, List<UtilityBill>? utilities)
        {
            var query = _context.Transactions
                        .Include(t => t.TransactionCategory)
                        .Include(t => t.UtilityBill)
                        .Where(t => t.MonthFor == month && t.YearFor == year);


            if (propertyId.HasValue)
                query = query.ByProperty(propertyId.Value);

            if (categoryId.HasValue)
                query = query.ByCategory(categoryId);

            query = query.ByUtilities(utilities);

            return await query.ToListAsync();
        }



        public async Task<List<Transaction>?> GetAllAsync()
        {
            return await _context.Transactions
                .ApplyRoleFilter(_currentuser, _context)
                .WithDetails()
                .OrderBy(t => t.CreatedOn)
                .ToListAsync();
        }


        public async Task<Transaction?> GetByExpenseIdAsync(int id)
        {
            return await _context.Transactions
                .ApplyRoleFilter(_currentuser, _context)
                .Where(u => u.ExpenseId == id)
                .WithDetails()
                .OrderBy(t => t.CreatedOn)
                .FirstOrDefaultAsync();
        }



        public async Task<List<Transaction>?> GetByUserIdAsync(int userId)
        {
            return await _context.Transactions
                .Where(u => u.UserId == userId)
                .ApplyRoleFilter(_currentuser, _context)
                .WithDetails()
                .ToListAsync();
        }


        public async Task<int> UpdateAsync()
        {
            var changes = await _context.SaveChangesAsync();

            return changes;
        }



        public async Task<List<TenantBalanceDto>> GetBalancesAsync()
        {
            // ================================
            // 1. AGGREGATE (SQL ONLY)
            // ================================
            var aggregated = await _context.Transactions
                .WithUserOnly()
                .GroupBy(t => new
                {
                    UserId = t.UserId!.Value,
                    t.MonthFor,
                    t.YearFor,
                    t.TransactionCategoryId
                })
                .Select(g => new
                {
                    g.Key.UserId,
                    g.Key.MonthFor,
                    g.Key.YearFor,
                    CategoryId = g.Key.TransactionCategoryId,

                    TotalCharges = g.Sum(t =>
                        t.TransactionType.Item == "Charge" ? t.Amount : 0),

                    TotalPayments = g.Sum(t =>
                        t.TransactionType.Item == "Payment" ? t.Amount : 0)
                })
                .Where(x => x.TotalCharges - x.TotalPayments > 0)
                .OrderBy(x => x.YearFor)
                .ThenBy(x => x.MonthFor)
                .ToListAsync();


            if (!aggregated.Any())
                return new List<TenantBalanceDto>();


            // ================================
            // 2. LOAD LOOKUPS (SEPARATE QUERIES)
            // ================================
            var userIds = aggregated.Select(x => x.UserId).Distinct().ToList();
            var categoryIds = aggregated.Select(x => x.CategoryId).Distinct().ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var categories = await _context.SystemCodeItems
                .Where(c => categoryIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            // Units are linked via Transaction.UnitId (NOT Tenant)
            var unitMap = await _context.Transactions
                .Where(t => userIds.Contains(t.UserId!.Value))
                .Select(t => new { t.UserId, t.UnitId })
                .Distinct()
                .Join(
                    _context.Units.Include(u => u.Property),
                    tu => tu.UnitId,
                    u => u.Id,
                    (tu, u) => new { tu.UserId, Unit = u }
                )
                .GroupBy(x => x.UserId!.Value)
                .ToDictionaryAsync(g => g.Key, g => g.First().Unit);


            // ================================
            // 3. FINAL PROJECTION (IN MEMORY)
            // ================================
            var result = aggregated.Select(a =>
            {
                users.TryGetValue(a.UserId, out User? user);
                categories.TryGetValue(a.CategoryId, out SystemCodeItem? category);
                unitMap.TryGetValue(a.UserId, out Unit? unit);

                return new TenantBalanceDto
                {
                    UserId = a.UserId,
                    FullName = user != null
                        ? user.FirstName + " " + user.LastName
                        : "",

                    UnitName = unit != null ? unit.Name : "",
                    PropertyName = unit?.Property != null ? unit.Property.Name : "",

                    CategoryId = a.CategoryId,
                    CategoryName = category != null ? category.Item : "",

                    Month = a.MonthFor,
                    Year = a.YearFor,

                    TotalCharges = a.TotalCharges,
                    TotalPayments = a.TotalPayments,
                    Balance = a.TotalCharges - a.TotalPayments
                };
            })
            .ToList();

            return result;
        }




        public async Task<List<TenantBalanceDto>> GetBalanceByUserAsync(int userId)
        {
            var chargeTypeId = await _context.SystemCodeItems
                .Where(x => x.Item == "Charge")
                .Select(x => x.Id)
                .FirstAsync();

            var paymentTypeId = await _context.SystemCodeItems
                .Where(x => x.Item == "Payment")
                .Select(x => x.Id)
                .FirstAsync();

            var data = await _context.Transactions
                .ApplyRoleFilter(_currentuser, _context)
                .WithUserOnly()
                .Where(t => t.UserId == userId)
                .GroupBy(t => new
                {
                    t.UserId,
                    t.MonthFor,
                    t.YearFor,
                    t.TransactionCategoryId
                })
                .Select(g => new
                {
                    g.Key.UserId,
                    g.Key.MonthFor,
                    g.Key.YearFor,
                    g.Key.TransactionCategoryId,

                    TotalCharges = g
                        .Where(t => t.TransactionTypeId == chargeTypeId)
                        .Sum(t => t.Amount),

                    TotalPayments = g
                        .Where(t => t.TransactionTypeId == paymentTypeId)
                        .Sum(t => t.Amount)
                })
                .Where(x => x.TotalCharges - x.TotalPayments > 0)
                .OrderBy(x => x.YearFor)
                .ThenBy(x => x.MonthFor)
                .ToListAsync();

            // Enrich names AFTER SQL
            var categories = await _context.SystemCodeItems
                .ToDictionaryAsync(x => x.Id, x => x.Item);

            return data.Select(x => new TenantBalanceDto
            {
                UserId = x.UserId!.Value,
                Month = x.MonthFor,
                Year = x.YearFor,
                CategoryId = x.TransactionCategoryId,
                CategoryName = categories.GetValueOrDefault(x.TransactionCategoryId, ""),
                TotalCharges = x.TotalCharges,
                TotalPayments = x.TotalPayments,
                Balance = x.TotalCharges - x.TotalPayments
            }).ToList();
        }



        public async Task<List<TenantBalanceDto>> GetBalanceByUtillityAsync(
            int utilityBillId,
            BalanceFilter? filter = null)
        {
            var query = _context.Transactions
                .ApplyRoleFilter(_currentuser, _context)
                .WithUserOnly()
                .Where(t => t.UtilityBillId == utilityBillId);

            if (filter?.MonthFor != null)
                query = query.Where(t => t.MonthFor == filter.MonthFor.Value);

            if (filter?.YearFor != null)
                query = query.Where(t => t.YearFor == filter.YearFor.Value);

            if (filter?.UserId != null)
                query = query.Where(t => t.UserId == filter.UserId.Value);

            return await query
                .GroupBy(t => new TenantBalanceGroupKey
                (
                    t.UserId!.Value,
                    t.MonthFor,
                    t.YearFor,
                    t.TransactionCategoryId
                ))
                .ToTenantBalance()
                .WithPositiveBalance()
                .OrderByPeriod()
                .ToListAsync();
        }



    }
}
