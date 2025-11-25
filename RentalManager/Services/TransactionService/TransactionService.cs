using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.InvoiceLine;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.InvoiceRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.SystemCodeRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.InvoiceService;
using System.Diagnostics;

namespace RentalManager.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly ISystemCodeRepository _systemcoderepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IUnitRepository _unitrepo;
        private readonly ITenantRepository _tenantrepo;
        private readonly IUserRepository _userrepo;
        private readonly IInvoiceRepository _invoicerepo;
        private readonly IInvoiceService _invoiceservice;
        private readonly IUtilityBillRepository _utilitybillrepo;

        public TransactionService(
            ITransactionRepository repo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ISystemCodeRepository systemcoderepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ITenantRepository tenantrepo,
            IUserRepository userrepo,
            IInvoiceRepository invoiceRepo,
            IInvoiceService invoiceservice,
            IUtilityBillRepository utilitybillrepo)
        {
            _repo = repo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _systemcoderepo = systemcoderepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _tenantrepo = tenantrepo;
            _userrepo = userrepo;
            _invoicerepo = invoiceRepo;
            _invoiceservice = invoiceservice;
            _utilitybillrepo = utilitybillrepo;
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



        public async Task<ApiResponse<READTransactionDto>> AddCharge(CREATEIncoiceTransactionDto createdCharge)
        {
            if (createdCharge.Item == null || !createdCharge.Item.Any())
                return new ApiResponse<READTransactionDto>(null, "Choose at least one item");

            var user = await _userrepo.GetByIdAsync(createdCharge.UserId);
            if (user == null)
                return new ApiResponse<READTransactionDto>(null, "User does not exist");

            var tenant = await _tenantrepo.GetByUserIdAsync(user.Id);
            if (tenant == null)
                return new ApiResponse<READTransactionDto>(null, "Tenant does not exist");

            var transactionType = await _systemcodeitemrepo.GetByItemAsync("charge");
            if (transactionType == null)
                return new ApiResponse<READTransactionDto>(null, "Transaction type 'charge' not found");

            try
            {
                var transactions = new List<Transaction>();
                decimal totalAmount = 0;

                foreach (var item in createdCharge.Item)
                {
                    var category = await _systemcodeitemrepo.GetByIdAsync(item.TransactionCategory);
                    if (category == null)
                        return new ApiResponse<READTransactionDto>(null, $"Category {item.TransactionCategory} not found");

                    var transactionDto = new CREATETransactionDto
                    {
                        UserId = createdCharge.UserId,
                        PropertyId = user.PropertyId,
                        UnitId = tenant.UnitId,
                        TransactionTypeId = transactionType.Id,
                        TransactionCategoryId = category.Id,
                        Amount = item.Amount,
                        MonthFor = createdCharge.MonthFor,
                        YearFor = createdCharge.YearFor,
                        TransactionDate = createdCharge.InvoiceDate,
                        Notes = createdCharge.Notes
                    };

                    var transactionEntity = transactionDto.ToEntity();
                    transactions.Add(transactionEntity);
                    totalAmount += item.Amount;
                }

                // Save all transactions in one go (atomic)
                await _repo.AddRangeAsync(transactions);

                // ✅ Now create the invoice
                var invoiceDto = new CREATEInvoiceDto
                {
                    TotalAmount = totalAmount,
                    Status = "Unpaid",
                    Combine = createdCharge.Combine,
                    TransactionId = transactions.First().Id
                };

                var invoiceLines = createdCharge.Item.Select(i => new CREATEInvoiceLineDto
                {
                    TransactionCategory = i.TransactionCategory,
                    Amount = i.Amount
                }).ToList();

                var invoice = await _invoiceservice.Add(invoiceDto, invoiceLines);

                return new ApiResponse<READTransactionDto>(null, "Charge and invoice created successfully");
            }

            catch(Exception ex)
            {
                return new ApiResponse<READTransactionDto>(null, ex.InnerException?.Message ?? ex.Message);
            }


        }



        public async Task<ApiResponse<bool>> GenerateRentInvoices(int propertyId)
        {
            try
            {
                // 1. Validate property
                var property = await _propertyrepo.GetByIdAsync(propertyId);
                if (property == null)
                    return new ApiResponse<bool>(false, "Property does not exist");

                var month = DateTime.UtcNow.Month;
                var year = DateTime.UtcNow.Year;

                // 2. Get active tenants once
                var tenants = await _tenantrepo.GetAllByPropertyId(propertyId, true);
                if (tenants is null)
                    return new ApiResponse<bool>(false, "No active tenants found for this property");

                // 3. Get existing charge for current month in one query
                var existingCharges = await _repo.FindByMonthAsync(month, year, propertyId, "rent");

                // 4. Generate invoices depending on situation
                List<CREATEIncoiceTransactionDto> invoiceDtos;


                if (existingCharges == null || !existingCharges.Any())
                {
                    // First rent invoice creation
                    invoiceDtos = CreateFullRentInvoice(tenants);
                }
                else
                {
                    invoiceDtos = CreateRentBalanceInvoice(tenants, existingCharges);
                }


                // Nothing to save
                if (!invoiceDtos.Any())
                    return new ApiResponse<bool>(true, "Nothing to generate");

                var tenantLookup = tenants.ToDictionary(t => t.User.Id, t => t);

                // get transactionCategory = "charge", transactionType = "rent"
                var type = await _systemcodeitemrepo.GetByItemAsync("charge");
                var category = await _systemcodeitemrepo.GetByItemAsync("rent");

                // Convert DTOs to Entities
                var entities = invoiceDtos.Select(dto =>
                {
                    var tenant = tenantLookup[dto.UserId];

                    var ctx = new InvoiceMappingContext
                    {
                        PropertyId = propertyId,
                        UnitId = tenant.Unit.Id,
                        TransactionTypeId = type.Id,
                        TransactionCategoryId = category.Id
                    };

                    return dto.ToEntity(ctx);
                }).ToList();

                var added = await _repo.AddRangeAsync(entities);

                return added > 0
                    ? new ApiResponse<bool>(true, "Rent invoices generated successfully")
                    : new ApiResponse<bool>(false, "Failed to generate invoices");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false,
                    $"Error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<bool>> GenerateUtilityBillInvoices(int propertyId)
        {
            try
            {
                // 1. Validate property
                var property = await _propertyrepo.GetByIdAsync(propertyId);
                if (property == null)
                    return new ApiResponse<bool>(false, "Property does not exist");

                var month = DateTime.UtcNow.Month;
                var year = DateTime.UtcNow.Year;

                // 2. Get active tenants once
                var tenants = await _tenantrepo.GetAllByPropertyId(propertyId, true);
                if (tenants is null)
                    return new ApiResponse<bool>(false, "No active tenants found for this property");

                // 3. Get existing charge for current month in one query
                List<CREATEIncoiceTransactionDto> invoiceDtos;
                bool isReccurring = true;

                var utilities = await _utilitybillrepo.GetByPropertyIdAsync(property.Id, isReccurring);

                var existingCharges = await _repo.FindByMonthAsync(month, year, propertyId, "", utilities);


                if(utilities is null)
                    return new ApiResponse<bool>(true, "No utilities available");

                if (existingCharges == null || !existingCharges.Any())
                {
                    // First rent invoice creation
                    invoiceDtos = CreateFullUtilityInvoices(tenants, utilities);
                }
                else
                {
                    invoiceDtos = CreateUtilityBalanceInvoices(tenants, utilities, existingCharges);
                }


                // Nothing to save
                if (!invoiceDtos.Any())
                    return new ApiResponse<bool>(true, "UtilityBill invoices already exists");



                var transactionType = await _systemcodeitemrepo.GetByItemAsync("charge");

                var systemCategories = await _systemcodeitemrepo.GetByCodeAsync("TRANSACTIONCATEGORY");
                var systemCategoryLookup = systemCategories
                                                            .GroupBy(c => c.Item.ToLower())
                                                            .ToDictionary(g => g.Key, g => g.First().Id);


                var utilityLookup = utilities.ToDictionary(u => u.Name.ToLower(), u => u.Id);

                var tenantLookup = tenants.ToDictionary(t => t.User.Id, t => t);

                var entities = new List<Transaction>();

                foreach (var dto in invoiceDtos)
                {
                    var tenant = tenantLookup[dto.UserId];

                    foreach (var item in dto.Item)
                    {
                        if (!systemCategoryLookup.TryGetValue(item.TransactionCategory.ToLower(), out var categoryId))
                        {
                            var SystemCode = await _systemcoderepo.GetByCodeAsync("TRANSACTIONCATEGORY");
                            var newItem = await _systemcodeitemrepo.AddAsync(new SystemCodeItem
                            {
                                SystemCodeId = SystemCode.Id,
                                Item = item.TransactionCategory
                            });

                            categoryId = newItem.Id;
                            systemCategoryLookup[item.TransactionCategory] = categoryId;
                        }

                        var utilityBillId = utilityLookup[item.TransactionCategory.ToLower()];

                        var ctx = new InvoiceMappingContext
                        {
                            PropertyId = propertyId,
                            UnitId = tenant.Unit.Id,
                            TransactionTypeId = transactionType.Id,
                            TransactionCategoryId = categoryId,
                            UtilityBillId = utilityBillId
                        };

                        entities.Add(dto.ToEntity(ctx, item.Amount));
                    }
                }


                var added = await _repo.AddRangeAsync(entities);

                return added > 0
                    ? new ApiResponse<bool>(true, "Utility invoices generated successfully")
                    : new ApiResponse<bool>(false, "Failed to generate invoices");

            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false,
                    $"Error occurred: {ex.InnerException?.Message ?? ex.Message}");
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



        public async Task<ApiResponse<List<TenantBalanceDto>>> GetUserBalances(int userId)
        {
            try
            {
                var balances = await _repo.GetBalanceByUserAsync(userId);

                if (!balances.Any())
                    return new ApiResponse<List<TenantBalanceDto>>(null, "Tenant has no balance.");


                return new ApiResponse<List<TenantBalanceDto>>(balances, "Balances retrieved successfully.");
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
                // 1. Validate tenant
                var tenantPaid = await _tenantrepo.GetByIdAsync(payment.TenantId);
                if (tenantPaid == null)
                    return new ApiResponse<READTransactionDto>(null, "Tenant not found");

                // 2. Get all outstanding balances for that tenant
                var balances = await _repo.GetBalanceByUserAsync(tenantPaid.UserId);

                if (balances == null || !balances.Any())
                {
                    // No debts → record as advance payment
                    var transactionType = await _systemcodeitemrepo.GetByItemAsync("payment");

                    var advanceTx = CreateAdvancePaymentTransaction(payment, tenantPaid, transactionType.Id);
                    await _repo.AddAsync(advanceTx);

                    return new ApiResponse<READTransactionDto>(advanceTx.ToReadDto(), "Tenant had no debt. Payment saved as advance.");
                }

                // Order debts oldest first (Year, then Month)
                balances = balances.OrderBy(b => b.Year).ThenBy(b => b.Month).ToList();


                // Generate transactions
                var newTransactions = await CreatePaymentTransactions(
                    balances,
                    tenantPaid,
                    payment.Amount
                );


                // Save to DB
                var result = await _repo.AddRangeAsync(newTransactions);
                

                return result > 0
                    ? new ApiResponse<READTransactionDto>(null, "Payment applied successfully")
                    : new ApiResponse<READTransactionDto>(null, "Failed to add payment(s)");

            }
            catch (Exception ex)
            {
                return new ApiResponse<READTransactionDto>(
                    null,
                    $"Error Occurred: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
        }





                                    // HELPER METHODS





        private List<CREATEIncoiceTransactionDto> CreateRentBalanceInvoice(List<Tenant> tenants, List<Transaction> existingCharges)
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            // Group charges by user
            var chargesLookup = existingCharges
                .GroupBy(c => c.UserId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var invoiceList = new List<CREATEIncoiceTransactionDto>();

            foreach (var tenant in tenants)
            {
                var tenantId = tenant.User.Id;
                var monthlyRent = tenant.Unit.Amount;

                // Get existing charges
                chargesLookup.TryGetValue(tenantId, out var tenantCharges);

                // no charges exist -> full invoice
                if (tenantCharges == null || tenantCharges.Count == 0)
                {
                    invoiceList.Add(new CREATEIncoiceTransactionDto
                    {
                        UserId = tenantId,
                        MonthFor = month,
                        YearFor = year,
                        InvoiceDate = DateTime.UtcNow,
                        Item = new List<CREATEItemDto>
                {
                    new CREATEItemDto
                    {
                        TransactionCategory = "Rent",
                        Amount = monthlyRent
                    }
                }
                    });
                    continue;
                }

                var totalCharged = tenantCharges.Sum(c => c.Amount);

                // partial paid → invoice only balance
                if (totalCharged < monthlyRent)
                {
                    var balance = monthlyRent - totalCharged;

                    invoiceList.Add(new CREATEIncoiceTransactionDto
                    {
                        UserId = tenantId,
                        MonthFor = month,
                        YearFor = year,
                        InvoiceDate = DateTime.UtcNow,
                        Item = new List<CREATEItemDto>
                {
                    new CREATEItemDto
                    {
                        TransactionCategory = "Rent",
                        Amount = balance
                    }
                }
                    });
                }

                // fully paid → do nothing
            }

            return invoiceList;
        }


        private List<CREATEIncoiceTransactionDto> CreateFullRentInvoice(List<Tenant> tenants)
        {

            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            return tenants.Select(t => new CREATEIncoiceTransactionDto
            {
                UserId = t.User.Id,
                MonthFor = month,
                YearFor = year,
                InvoiceDate = DateTime.UtcNow,
                Item = new List<CREATEItemDto>
        {
            new CREATEItemDto
            {
                TransactionCategory = "Rent",
                Amount = t.Unit.Amount
            }
        }
            }).ToList();

        }


        private List<CREATEIncoiceTransactionDto> CreateFullUtilityInvoices(
            List<Tenant> tenants,
            List<UtilityBill> utilities)
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            var invoices = new List<CREATEIncoiceTransactionDto>();

            // If no utilities, return empty list
            if (utilities == null || utilities.Count == 0)
                return invoices;

            foreach (var tenant in tenants)
            {
                // Create invoice items — one entry for each utility bill
                var items = utilities.Select(u => new CREATEItemDto
                {
                    TransactionCategory = u.Name,
                    Amount = u.Amount
                }).ToList();

                var invoice = new CREATEIncoiceTransactionDto
                {
                    UserId = tenant.User.Id,
                    MonthFor = month,
                    YearFor = year,
                    InvoiceDate = DateTime.UtcNow,
                    Item = items
                };

                invoices.Add(invoice);
            }

            return invoices;
        }


        private List<CREATEIncoiceTransactionDto> CreateUtilityBalanceInvoices(
            List<Tenant> tenants,
            List<UtilityBill> utilities,
            List<Transaction> existingCharges)
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            var invoices = new List<CREATEIncoiceTransactionDto>();

            if (utilities == null || utilities.Count == 0)
                return invoices;

            // Build lookup: { UserId -> List<Transaction> }
            var chargesLookup = existingCharges
                .GroupBy(t => t.UserId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var tenant in tenants)
            {
                var userId = tenant.User.Id;
                chargesLookup.TryGetValue(userId, out var tenantCharges);

                List<CREATEItemDto> itemsToBill = new();

                foreach (var utility in utilities)
                {
                    // Does this tenant already have a charge entry for this utility?
                    bool hasCharge =
                        tenantCharges?.Any(c => c.TransactionCategory.Item.ToLower() == utility.Name.ToLower()) ?? false;

                    if (!hasCharge)
                    {
                        // Utility missing → bill full amount
                        itemsToBill.Add(new CREATEItemDto
                        {
                            TransactionCategory = utility.Name,
                            Amount = utility.Amount
                        });
                    }
                }

                // If all utilities already billed → skip tenant
                if (!itemsToBill.Any())
                    continue;

                // Create invoice for missing utilities
                invoices.Add(new CREATEIncoiceTransactionDto
                {
                    UserId = userId,
                    MonthFor = month,
                    YearFor = year,
                    InvoiceDate = DateTime.UtcNow,
                    Item = itemsToBill
                });
            }

            return invoices;
        }



        private Transaction CreateAdvancePaymentTransaction(CREATEPaymentDto payment, Tenant tenant, int transactionTypeId)
        {

            return new Transaction
            {
                UserId = tenant.User.Id,
                PropertyId = tenant.Unit.PropertyId,
                UnitId = tenant.Unit.Id,
                TransactionDate = DateTime.UtcNow,
                MonthFor = DateTime.UtcNow.Month,
                YearFor = DateTime.UtcNow.Year,
                Amount = payment.Amount,
                TransactionTypeId = transactionTypeId,
                TransactionCategoryId = transactionTypeId,
                Notes = "Advance payment"
            };
        }


        

        private async Task<List<Transaction>> CreatePaymentTransactions(
            List<TenantBalanceDto> balances,
            Tenant tenant,
            decimal amountPaid)
        {
            var newTransactions = new List<Transaction>();

            // Look up transaction type once
            var transactionType = await _systemcodeitemrepo.GetByItemAsync("payment");

            // Loop through each balance in order
            foreach (var bal in balances)
            {
                if (amountPaid <= 0)
                    break;

                decimal payable = Math.Min(bal.Balance, amountPaid);
                amountPaid -= payable;

                // Create a normal payment transaction
                var tx = new Transaction
                {
                    UserId = tenant.UserId,
                    PropertyId = tenant.User.PropertyId,
                    UnitId = tenant.UnitId,
                    TransactionDate = DateTime.UtcNow,
                    MonthFor = bal.Month,
                    YearFor = bal.Year,
                    Amount = payable,
                    TransactionTypeId = transactionType.Id,
                    TransactionCategoryId = bal.CategoryId,   // Make sure you use ID, not name
                    Notes = "Payment recorded"
                };

                newTransactions.Add(tx);
            }

            // If still money left → create advance payment
            if (amountPaid > 0)
            {
                var advanceTx = CreateAdvancePaymentTransaction(
                    new CREATEPaymentDto
                    {
                        Amount = amountPaid,
                        TenantId = tenant.Id
                    },
                    tenant,
                    transactionType.Id
                );

                newTransactions.Add(advanceTx);
            }

            return newTransactions;
        }

    }
}
