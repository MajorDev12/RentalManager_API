using RentalManager.DTOs.Expense;
using RentalManager.Models;

namespace RentalManager.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ApiResponse<READExpenseDto>> Add(CREATEExpenseDto dto);
        Task<ApiResponse<List<READExpenseDto>>> GetAll();
        Task<ApiResponse<READExpenseDto>> GetById(int id);
        Task<ApiResponse<READExpenseDto>> Update(int expenseId, UPDATEExpenseDto dto);
        Task<ApiResponse<READExpenseDto>> Delete(int id);
    }
}
