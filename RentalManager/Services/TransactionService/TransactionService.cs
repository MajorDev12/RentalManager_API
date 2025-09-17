using RentalManager.DTOs.Property;
using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;

namespace RentalManager.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IUnitRepository _unitrepo;

        public TransactionService(
            ITransactionRepository repo,
            ISystemCodeItemRepository systemcodeitemrepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo) {
            _repo = repo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
        }


        public async Task<ApiResponse<List<READTransactionDto>>> GetAll()
        {
            try
            {
                var transactions = await _repo.GetAllAsync();

                if (transactions == null || transactions.Count == 0)
                {
                    return new ApiResponse<List<READTransactionDto>>(null, "Data Not Found.");
                }

                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READTransactionDto>>(transactionDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READTransactionDto>>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }



        public async Task<ApiResponse<READTransactionDto>> Add(CREATETransactionDto transaction)
        {

            try
            {
                // check if property exist
                var property = await _propertyrepo.GetByIdAsync(transaction.PropertyId);

                if (property == null)
                    return new ApiResponse<READTransactionDto>(null, "Property does not exist");

                // check if there is unit then confirm its fom the property above
                if (transaction.UnitId is int unitId)
                {
                    var unit = await _unitrepo.GetByIdAsync(unitId);

                    if (unit == null)
                        return new ApiResponse<READTransactionDto>(null, "Unit does not exist");

                    if (unit.PropertyId != property.Id)
                        return new ApiResponse<READTransactionDto>(null, "Unit is not from property selected");
                }

                // check if type, category and paymentMethod exist in systemCodeItems
                var type = await _systemcodeitemrepo.GetByIdAsync(transaction.TransactionTypeId);
                var category = await _systemcodeitemrepo.GetByIdAsync(transaction.TransactionCategoryId);

                if (type == null || category == null)
                    return new ApiResponse<READTransactionDto>(null, "Transaction type or category is not available");


                if (transaction.PaymentMethodId is int paymentMethodId)
                {
                    var paymentMethod = await _systemcodeitemrepo.GetByIdAsync(paymentMethodId);
                    if (paymentMethod == null)
                        return new ApiResponse<READTransactionDto>(null, "payment Method does not exist");

                }
                
                // month is 1 to 12 and year are numbers
                if(transaction.MonthFor is int month)
                {
                    if (month > 12 || month <= 0)
                        return new ApiResponse<READTransactionDto>(null, "choose correct month");
                }

                var transactionEntity = transaction.ToEntity();
                var addedTransaction = await _repo.AddAsync(transactionEntity);

                if (addedTransaction != null)
                {
                    return new ApiResponse<READTransactionDto>(null, "Transaction added Successfully");
                }
                else
                {
                    return new ApiResponse<READTransactionDto>(null, "Data Not Found");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<READTransactionDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READTransactionDto>> Update(int id, UPDATETransactionDto updatedTransaction)
        {
            try
            {
                var existingTransaction = await _repo.FindAsync(id);

                if (existingTransaction != null)
                {
                    var entity = updatedTransaction.UpdateEntity(existingTransaction);
                    var updated = await _repo.UpdateAsync();

                    if(updated <= 0)
                        return new ApiResponse<READTransactionDto>(null, "Failed To Update Transaction");

                    return new ApiResponse<READTransactionDto>(null, "Transaction Updated Successfully");

                }
                else
                {
                    return new ApiResponse<READTransactionDto>(null, "Transaction not available to update");
                }


            }
            catch (Exception ex)
            {
                return new ApiResponse<READTransactionDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READTransactionDto>> Delete(int id)
        {
            try
            {
                var transaction = await _repo.FindAsync(id);

                if (transaction == null)
                {
                    return new ApiResponse<READTransactionDto>("Transaction was not found");
                }

                await _repo.DeleteAsync(transaction);

                return new ApiResponse<READTransactionDto>(null, "Deleted Successfully");
            }
            catch(Exception ex)
            {
                return new ApiResponse<READTransactionDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<List<TenantBalanceDto>>> GetRentBalances()
        {
            try
            {
                var balances = await _repo.GetBalancesAsync();

                if (!balances.Any())
                    return new ApiResponse<List<TenantBalanceDto>>(null, "No unpaid tenants found.");


                return new ApiResponse<List<TenantBalanceDto>>(balances, "Unpaid tenants retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TenantBalanceDto>>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
