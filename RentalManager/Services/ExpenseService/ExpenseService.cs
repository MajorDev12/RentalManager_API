using RentalManager.DTOs.Expense;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Transaction;
using RentalManager.Helpers.Validations;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.ExpenseRepository;

namespace RentalManager.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;

        public ExpenseService(IExpenseRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<List<READExpenseDto>>> GetAll()
        {
            try
            {
                var expenses = await _repo.GetAllAsync();

                if (expenses == null || !expenses.Any()) 
                    return new ApiResponse<List<READExpenseDto>>(null, "Data Not Found");

                var expenseDtos = expenses.Select(p => p.ToReadDto()).ToList();
                return new ApiResponse<List<READExpenseDto>>(expenseDtos, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READExpenseDto>>($"error occured: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READExpenseDto>> GetById(int id)
        {
            try
            {
                var expense = await _repo.GetByIdAsync(id);

                if (expense == null) return new ApiResponse<READExpenseDto>("Data Not Found");

                var expenseDto = expense.ToReadDto();

                return new ApiResponse<READExpenseDto>(expenseDto, "Expense Fetched successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READExpenseDto>($"An error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READExpenseDto>> Add(CREATEExpenseDto dto)
        {
            try
            {

                if (dto == null) return new ApiResponse<READExpenseDto>("Missing Data Added");

                var expenseEntity = dto.ToEntity();

                var savedExpense = await _repo.AddAsync(expenseEntity);
                var expenseDto = savedExpense.ToReadDto();

                return new ApiResponse<READExpenseDto>(expenseDto, "Expense created successfully.");
            }
            catch (Exception ex)
            {
                // Optionally log the error here
                return new ApiResponse<READExpenseDto>($"An error occurred: {ex.Message}");
            }
        }




        public async Task<ApiResponse<READExpenseDto>> Update(int expenseId, UPDATEExpenseDto dto)
        {
            try
            {
                var existingExpense = await _repo.FindAsync(expenseId);

                if (existingExpense == null)
                    return new ApiResponse<READExpenseDto>(null, "Expense changed does not exist");

                // check if change was made
                bool hasChanges = ObjectComparer.HasChanges(existingExpense, dto, "UpdatedOn");

                if (!hasChanges)
                    return new ApiResponse<READExpenseDto>(null, "No changes detected.");


                var expenseEntity = dto.UpdateEntity(existingExpense);
                var updated = await _repo.UpdateAsync();

                if (updated <= 0)
                    return new ApiResponse<READExpenseDto>(null, "Failed To Update Expense");

                return new ApiResponse<READExpenseDto>(null, "Expense Updated Successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<READExpenseDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READExpenseDto>> Delete(int id)
        {
            try
            {
                var expense = await _repo.FindAsync(id);

                if (expense == null)
                {
                    return new ApiResponse<READExpenseDto>("Expense was not found");
                }

                await _repo.DeleteAsync(expense);

                return new ApiResponse<READExpenseDto>(null, "Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READExpenseDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



    }
}
