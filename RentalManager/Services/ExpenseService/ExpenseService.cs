using RentalManager.Data;
using RentalManager.DTOs.Expense;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.ExpenseRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IExpenseRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly ITransactionRepository _transactionrepo;
        private readonly ICurrentUser _currentuser;

        public ExpenseService(
            ApplicationDbContext context,
            IExpenseRepository repo,
            IPropertyRepository propertyrepo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ITransactionRepository transactionrepo,
            ICurrentUser currentuser)
        {
            _context = context;
            _repo = repo;
            _propertyrepo = propertyrepo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _transactionrepo = transactionrepo;
            _currentuser = currentuser;
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
            // 1. Validate property
            var property = await _propertyrepo.GetByIdAsync(_currentuser, dto.PropertyId);
            if (property == null)
                return ApiResponse<READExpenseDto>.FailResponse("Property does not exist");

            // 2. Resolve transaction type (WHAT)
            var transactionType = await _systemcodeitemrepo.GetByItemAsync("expense", "TRANSACTIONTYPE");
            var transactionCategory = await _systemcodeitemrepo.GetByItemAsync("expense", "TRANSACTIONCATEGORY");
            var expenseCategory = await _systemcodeitemrepo.GetByIdAsync(dto.ExpenseCategoryId);

            if (transactionType == null || transactionCategory == null || expenseCategory == null)
                return ApiResponse<READExpenseDto>.FailResponse("System Codes Not Configured");

            var now = DateTime.UtcNow;

            using var dbTx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 4. Create Expense entity
                var expenseEntity = dto.ToEntity();
                expenseEntity.AccountId = _currentuser.AccountId;

                var savedExpense = await _repo.AddAsync(expenseEntity);

                // 5. Create Transaction entity from Expense
                var transactionEntity = dto.ToTransactionEntity();

                transactionEntity.AccountId = _currentuser.AccountId;

                transactionEntity.ExpenseId = savedExpense.Id;
                transactionEntity.ExpenseCategoryId = expenseCategory.Id;

                transactionEntity.TransactionTypeId = transactionType.Id;
                transactionEntity.TransactionCategoryId = transactionCategory.Id;

                transactionEntity.MonthFor = now.Month;
                transactionEntity.YearFor = now.Year;

                await _transactionrepo.AddAsync(transactionEntity);

                await dbTx.CommitAsync();

                return ApiResponse<READExpenseDto>.SuccessResponse(
                    savedExpense.ToReadDto(),
                    "Expense created successfully"
                );
            }
            catch (Exception ex)
            {
                await dbTx.RollbackAsync();

                return ApiResponse<READExpenseDto>.FailResponse(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }




        public async Task<ApiResponse<READExpenseDto>> Update(int expenseId, UPDATEExpenseDto dto)
        {
            using var dbTx = await _context.Database.BeginTransactionAsync();

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

                var transactionUpdated = await _transactionrepo.GetByExpenseIdAsync(expenseEntity.Id);

                if (transactionUpdated == null)
                    return new ApiResponse<READExpenseDto>(null, "No Transaction Found For The Expense");

                // update Transaction
                var transactionEntity = dto.UpdateTransactionEntity(transactionUpdated);
                var updatedTransaction = await _transactionrepo.UpdateAsync();


                if (updated <= 0 || updatedTransaction <= 0)
                {
                    await dbTx.RollbackAsync();
                    return new ApiResponse<READExpenseDto>(null, "Failed To Update Expense");
                }

                await dbTx.CommitAsync();

                return new ApiResponse<READExpenseDto>(null, "Expense Updated Successfully");

            }
            catch (Exception ex)
            {
                await dbTx.RollbackAsync();
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
