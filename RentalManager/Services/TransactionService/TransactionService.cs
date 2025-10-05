using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.InvoiceLine;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.InvoiceRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Services.InvoiceService;
using System.Diagnostics;

namespace RentalManager.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IUnitRepository _unitrepo;
        private readonly ITenantRepository _tenantrepo;
        private readonly IInvoiceRepository _invoicerepo;
        private readonly IInvoiceService _invoiceservice;

        public TransactionService(
            ITransactionRepository repo,
            ISystemCodeItemRepository systemcodeitemrepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ITenantRepository tenantrepo,
            IInvoiceRepository invoiceRepo,
            IInvoiceService invoiceservice)
        {
            _repo = repo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _tenantrepo = tenantrepo;
            _invoicerepo = invoiceRepo;
            _invoiceservice = invoiceservice;
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

                // check if there is unit then confirm its from the property above
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

                if (type == null)
                    return new ApiResponse<READTransactionDto>(null, "Transaction type is not available");


                // check when category is -rent- confirm unit is selected
                //if (category.Item.ToLower() == "rent")
                //{
                //    if (transaction.UnitId is int id)
                //    {
                //        // check if unitAmount - totalAmount is 0 for whole monthFor
                //        var unit = await _unitrepo.FindAsync(id);
                //        var transactions = await _repo.GetAllAsync();
                //        var transactionForMonth = transactions?
                //            .Where(u => u.MonthFor == transaction.MonthFor && u.TransactionCategory.Item.ToLower() == "rent")
                //            .ToList();

                //        if(transactionForMonth.Any())
                //        {
                //            int initialAmount = 0;
                //            foreach(var monthFor in transactionForMonth)
                //            {
                //                monthFor.Amount += initialAmount;
                //            }

                //            if(unit.Amount - initialAmount <= 0)
                //            {
                //                return new ApiResponse<READTransactionDto>(null, $"Seems the Tenant has cleared Rent for this month {transaction.MonthFor} of this year {transaction.YearFor}");
                //            }
                //        }


                //    }
                //    else 
                //    { 
                //        return new ApiResponse<READTransactionDto>(null, "Please Add Unit For rent youre paying for");
                //    }

                //}



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
                    var createInvoiceDto = addedTransaction.ToInvoice();
                    var addedInvoice = await _invoiceservice.Add(createInvoiceDto, new List<CREATEInvoiceLineDto>());
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



        public async Task<ApiResponse<READTransactionDto>> AddPayment(CREATEPaymentDto payment)
        {
            try
            {
                // get tenant
                var tenantPaid = await _tenantrepo.FindAsync(payment.TenantId);

                if (tenantPaid == null)
                    return new ApiResponse<READTransactionDto>(null, "tenant not found");

                if (tenantPaid.UnitId == null)
                    return new ApiResponse<READTransactionDto>(null, "Tenant Not Assigned Unit");

                // change to Entity
                var paymentEntity = payment.ToPaymentEntity(tenantPaid);
                var transaction = new ApiResponse<Transaction>();


                if (paymentEntity.UtilityBill.isReccuring)
                {
                    transaction = await AddReccuringPayment(paymentEntity);

                    if (transaction.Data == null)
                        return new ApiResponse<READTransactionDto>(null, $"{transaction.Message}");

                    // Create Invoice
                    var createInvoiceDto = transaction.Data.ToInvoice();
                    var addedInvoice = await _invoiceservice.Add(createInvoiceDto, new List<CREATEInvoiceLineDto>());
                }
                else
                {
                    transaction.Data = await _repo.AddAsync(paymentEntity);
                    var createInvoiceDto = transaction.Data.ToInvoice();
                    await _invoiceservice.Add(createInvoiceDto, new List<CREATEInvoiceLineDto>());
                }

                return new ApiResponse<READTransactionDto>(transaction.Data.ToReadDto(), "");


            }
            catch (Exception ex) 
            {
                return new ApiResponse<READTransactionDto>(null, $"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

            
        public async Task<ApiResponse<Transaction>> AddReccuringPayment(Transaction payment)
        {
            try
            {
                // check if utility paidFor has balance, calc amount - balance, add transaction and update invoice
                if (payment.UtilityBillId is int utilityBillId)
                {
                    var balance = await _repo.GetBalanceByUtillityAsync(utilityBillId, new BalanceFilter { MonthFor = payment.MonthFor, YearFor = payment.YearFor});
                    if (!balance.Any())
                        return new ApiResponse<Transaction>(null, "Tenant Already Completed For This Month.");

                    // if balance, add payment
                    var addedTransaction = await _repo.AddAsync(payment);
                    return new ApiResponse<Transaction>(addedTransaction, "");
                }
                else
                {
                    return new ApiResponse<Transaction>(null, "UtilityBill Not Found.");
                }

                
            }catch (Exception ex)
            {
                return new ApiResponse<Transaction>(null, $"Error Occur: {ex.InnerException?.Message ?? ex.Message}");
            }


        }


    }
}
