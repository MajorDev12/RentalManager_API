using RentalManager.Models;

namespace RentalManager.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<List<Expense>?> GetAllAsync();

        Task<Expense?> GetByIdAsync(int id);

        Task<Expense> AddAsync(Expense expense);

        Task<int> UpdateAsync();

        Task DeleteAsync(Expense expense);

        Task<Expense?> FindAsync(int id);
    }
}
