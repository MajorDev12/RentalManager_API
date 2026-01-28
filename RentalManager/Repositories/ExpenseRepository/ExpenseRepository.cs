using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentuser;


        public ExpenseRepository(ApplicationDbContext context, ICurrentUser currentuser)
        {
            _context = context;
            _currentuser = currentuser;
        }


        public async Task<List<Expense>?> GetAllAsync()
        {
            var expenses = await _context.Expenses
                .ApplyRoleFilter(_currentuser, _context)
                .Include(p => p.Property)
                .ToListAsync();
            return expenses;
        }



        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }



        public async Task<Expense> AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return expense;
        }



        public async Task<int> UpdateAsync()
        {
            var changes = await _context.SaveChangesAsync();

            return changes;
        }



        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }



        public async Task<Expense?> FindAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }



    }
}
