using RentalManager.DTOs.InvoiceLine;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.SystemCodeRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.AccountAccessService;

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
        private readonly IUtilityBillRepository _utilitybillrepo;
        private readonly ICurrentUser _currentuser;

        public TransactionService(
            ITransactionRepository repo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ISystemCodeRepository systemcoderepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ITenantRepository tenantrepo,
            IUserRepository userrepo,
            IUtilityBillRepository utilitybillrepo,
            ICurrentUser currentuser)
        {
            _repo = repo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _systemcoderepo = systemcoderepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _tenantrepo = tenantrepo;
            _userrepo = userrepo;
            _utilitybillrepo = utilitybillrepo;
            _currentuser = currentuser;
        }


        public async Task<ApiResponse<List<READTransactionDto>>> GetAll()
        {
            try
            {
                var transactions = await _repo.GetAllAsync();

                if (transactions == null)
                    return ApiResponse<List<READTransactionDto>>.FailResponse("Something went wrong.");

                if(transactions.Count == 0)
                    return ApiResponse<List<READTransactionDto>>.SuccessResponse(null, "No Items Found.");

                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return ApiResponse<List<READTransactionDto>>.SuccessResponse(transactionDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READTransactionDto>>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<List<READTransactionDto>>> GetByUser(int domainUserId)
        {
            try
            {
                var transactions = await _repo.GetByUserIdAsync(domainUserId);

                if (transactions == null)
                    return ApiResponse<List<READTransactionDto>>.FailResponse("Something went wrong.");

                if (transactions.Count == 0)
                    return ApiResponse<List<READTransactionDto>>.SuccessResponse(null, "No Items Found.");


                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return ApiResponse<List<READTransactionDto>>.SuccessResponse(transactionDtos, "");
            }
            catch (Exception ex)
            {
                
                return ApiResponse<List<READTransactionDto>>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<List<READTransactionDto>>> GetByTenantId(int tenantId)
        {
            try
            {
                var domainUserId = await _tenantrepo.GetUserIdByTenantIdAsync(tenantId);

                if (domainUserId == null) return ApiResponse<List<READTransactionDto>>.FailResponse("Something went wrong.Tenant Id Does Not Exist");

                var transactions = await _repo.GetByUserIdAsync(domainUserId);

                if (transactions == null)
                    return ApiResponse<List<READTransactionDto>>.FailResponse("Something went wrong.");

                if (transactions.Count == 0)
                    return ApiResponse<List<READTransactionDto>>.SuccessResponse(null, "No Items Found.");


                var transactionDtos = transactions.Select(it => it.ToReadDto()).ToList();

                return ApiResponse<List<READTransactionDto>>.SuccessResponse(transactionDtos, "");
            }
            catch (Exception ex)
            {

                return ApiResponse<List<READTransactionDto>>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<READTransactionDto>> Add(CREATETransactionDto transaction)
        {

            try
            {
                // check if property exist
                var property = await _propertyrepo.GetByIdAsync(transaction.PropertyId.Value);

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

                if (addedTransaction == null)
                {
                    return new ApiResponse<READTransactionDto>(null, "Something went wrong");
                }

                    return new ApiResponse<READTransactionDto>(null, "Transaction added Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READTransactionDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READTransactionDto>> AddCharge(CREATEIncoiceTransactionDto createdCharge)
        {
            if (createdCharge.Item == null || !createdCharge.Item.Any())
                return ApiResponse<READTransactionDto>.FailResponse("Choose at least one item");

            var user = await _userrepo.GetByIdAsync(createdCharge.UserId);
            if (user == null)
                return ApiResponse<READTransactionDto>.FailResponse("User does not exist");

            var tenant = await _tenantrepo.GetByUserIdAsync(user.Id);
            if (tenant == null)
                return ApiResponse<READTransactionDto>.FailResponse("Tenant does not exist");

            var transactionType = await _systemcodeitemrepo.GetByCodeAndItemAsync("charge");
            if (transactionType == null)
                return ApiResponse<READTransactionDto>.FailResponse("Transaction type 'charge' not found");

            try
            {
                var transactions = new List<Transaction>();

                foreach (var item in createdCharge.Item)
                {
                    var category = await _systemcodeitemrepo.GetByIdAsync(item.TransactionCategoryId);
                    if (category == null)
                        return ApiResponse<READTransactionDto>.FailResponse( $"GroupKey {item.TransactionCategoryId} not found");

                    UtilityBill? utility = null;

                    if (category.Item == "utility")
                    {
                        if (!item.UtilityBillId.HasValue)
                            return ApiResponse<READTransactionDto>
                                .FailResponse("Utility bill is required for utility charges");

                        utility = await _utilitybillrepo
                            .GetByIdAsync(item.UtilityBillId.Value);

                        if (utility == null)
                            return ApiResponse<READTransactionDto>
                                .FailResponse("Utility bill not found");
                    }




                    var transaction = new Transaction
                    {
                        AccountId = _currentuser.AccountId,
                        UserId = createdCharge.UserId,
                        PropertyId = user.PropertyId,
                        UnitId = tenant.UnitId,
                        TransactionTypeId = transactionType.Id,
                        TransactionCategoryId = category.Id,
                        Amount = item.Amount,
                        MonthFor = createdCharge.MonthFor,
                        YearFor = createdCharge.YearFor,
                        TransactionDate = createdCharge.InvoiceDate,
                        Notes = createdCharge.Notes,
                    };

                    transactions.Add(transaction);
                }

                // Save all transactions in one go (atomic)
                await _repo.AddRangeAsync(transactions);

                return ApiResponse<READTransactionDto>.SuccessResponse(null, "Invoice Created Successfully");
            }

            catch(Exception ex)
            {
                return ApiResponse<READTransactionDto>.FailResponse(ex.InnerException?.Message ?? ex.Message);
            }


        }


        public async Task<ApiResponse<READTransactionDto>> AddPayment(CREATEPaymentDto payment)
        {
            try
            {
                // 1. Validate tenant
                var tenantPaid = await _tenantrepo.GetByIdAsync(payment.TenantId);
                if (tenantPaid == null)
                    return ApiResponse<READTransactionDto>.FailResponse("Tenant not found");

                // 2. Get all outstanding balances for that tenant
                var balances = await _repo.GetBalanceByUserAsync(tenantPaid.UserId);

                var paymentType = await _systemcodeitemrepo.GetByCodeAndItemAsync("payment");
                if (paymentType == null)
                    return ApiResponse<READTransactionDto>.FailResponse("Payment transaction type not found");


                if (balances == null || !balances.Any())
                {
                    var advanceCategory = await _systemcodeitemrepo.GetByCodeAndItemAsync("advance", "TRANSACTIONCATEGORY");

                    if (advanceCategory == null)
                        return ApiResponse<READTransactionDto>.FailResponse("Failed to add Advance Payment");

                    var advanceTx = CreateAdvancePaymentTransaction(payment, tenantPaid, paymentType.Id, advanceCategory.Id);
                    await _repo.AddAsync(advanceTx);

                    return new ApiResponse<READTransactionDto>(advanceTx.ToReadDto(), "Tenant had no debt. Payment saved as advance.");
                }

                // Order debts oldest first (Year, then Month)
                balances = balances.OrderBy(b => b.Year).ThenBy(b => b.Month).ToList();


                // Generate transactions
                var newTransactions = await CreatePaymentTransactions(
                    balances,
                    tenantPaid,
                    payment.Amount,
                    paymentType.Id
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



        public async Task<ApiResponse<bool>> GenerateRentInvoices(int propertyId)
        {
            try
            {
                var now = DateTime.UtcNow;
                var month = now.Month;
                var year = now.Year;

                // 1. Validate property
                var property = await _propertyrepo.GetByIdAsync(propertyId);
                if (property == null)
                    return ApiResponse<bool>.FailResponse("Property does not exist");

                // 2. Get active tenants once
                var tenants = await _tenantrepo.GetAllByPropertyId(propertyId, true);
                if (tenants == null || tenants.Count == 0)
                    return ApiResponse<bool>.FailResponse("No active tenants found");

                // 3. Resolve system codes ONCE
                var chargeType = await _systemcodeitemrepo.GetByCodeAndItemAsync("charge");
                if (chargeType == null)
                    return ApiResponse<bool>.FailResponse("Transaction type 'charge' not found");

                var rentCategory = await _systemcodeitemrepo.GetByCodeAndItemAsync("rent", "TRANSACTIONCATEGORY");
                if (rentCategory == null)
                    return ApiResponse<bool>.FailResponse("Transaction category 'rent' not found");

                // 4. Fetch existing rent charges for the period
                var existingCharges = await _repo.FindByMonthAsync(
                    month,
                    year,
                    propertyId,
                    rentCategory.Id
                );

                // 5. Generate invoice DTOs
                List<CREATEIncoiceTransactionDto> invoiceDtos;

                if (existingCharges == null || existingCharges.Count == 0)
                {
                    // No rent charges yet → full rent
                    invoiceDtos = CreateFullRentInvoice(tenants, rentCategory.Id, now);
                }
                else
                {
                    // Partial / missing rent → balance invoices
                    invoiceDtos = CreateRentBalanceInvoice(
                        tenants,
                        existingCharges,
                        rentCategory.Id,
                        now
                    );
                }

                // Nothing to save
                if (!invoiceDtos.Any())
                    return ApiResponse<bool>.SuccessResponse(true, "Invoices Already Created for this month");

                // 6. Tenant lookup for mapping
                var tenantLookup = tenants
                    .Where(t => t.User != null && t.Unit != null)
                    .ToDictionary(t => t.User.Id);

                // 7. Map DTOs → Transactions
                var entities = invoiceDtos.Select(dto =>
                {
                    var tenant = tenantLookup[dto.UserId];

                    var ctx = new InvoiceMappingContext
                    {
                        PropertyId = propertyId,
                        UnitId = tenant.Unit.Id,
                        TransactionTypeId = chargeType.Id,
                        TransactionCategoryId = rentCategory.Id
                    };

                    var entity = dto.ToEntity(ctx);
                    entity.AccountId = _currentuser.AccountId;

                    return entity;
                }).ToList();

                // 8. Persist atomically
                var added = await _repo.AddRangeAsync(entities);

                return added > 0
                    ? ApiResponse<bool>.SuccessResponse(true, "Rent invoices generated successfully")
                    : ApiResponse<bool>.FailResponse("Failed to generate rent invoices");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse(
                    $"Error occurred: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
        }



        public async Task<ApiResponse<bool>> GenerateUtilityBillInvoices(int propertyId)
        {
            try
            {
                var now = DateTime.UtcNow;
                var month = now.Month;
                var year = now.Year;

                // 1. Validate property
                var property = await _propertyrepo.GetByIdAsync(propertyId);
                if (property == null)
                    return ApiResponse<bool>.FailResponse("Property does not exist");

                // 2. Active tenants
                var tenants = await _tenantrepo.GetAllByPropertyId(propertyId, true);
                if (tenants == null || tenants.Count == 0)
                    return ApiResponse<bool>.FailResponse("No active tenants found");

                // 3. Recurring utilities
                var utilities = await _utilitybillrepo.GetByPropertyIdAsync(propertyId);

                if (utilities == null || utilities.Count == 0)
                    return ApiResponse<bool>.SuccessResponse(true, "No utilities to invoice");

                // 4. Resolve transaction type & category ONCE
                var chargeType = await _systemcodeitemrepo.GetByCodeAndItemAsync("charge", "TRANSACTIONTYPE");
                var utilityCategory = await _systemcodeitemrepo.GetByCodeAndItemAsync("utility", "TRANSACTIONCATEGORY");

                if (chargeType == null || utilityCategory == null)
                    return ApiResponse<bool>.FailResponse("Transaction codes not configured");

                // 5. Existing utility charges for this month
                var existingCharges = await _repo.FindByMonthAsync(
                    month,
                    year,
                    propertyId,
                    utilityCategory.Id
                );

                // 6. Generate invoice DTOs
                var invoiceDtos = existingCharges == null || !existingCharges.Any()
                    ? CreateFullUtilityInvoices(tenants, utilities, now)
                    : CreateUtilityBalanceInvoices(tenants, utilities, existingCharges, now);

                if (!invoiceDtos.Any())
                    return ApiResponse<bool>.SuccessResponse(true, "Utility invoices already exist");

                var tenantLookup = tenants
                    .Where(t => t.User != null && t.Unit != null)
                    .ToDictionary(t => t.User.Id);

                var entities = new List<Transaction>();

                // 7. Map DTOs → Transactions
                foreach (var dto in invoiceDtos)
                {
                    var tenant = tenantLookup[dto.UserId];

                    foreach (var item in dto.Item)
                    {
                        var entity = new Transaction
                        {
                            AccountId = _currentuser.AccountId,
                            UserId = tenant.User.Id,
                            PropertyId = propertyId,
                            UnitId = tenant.Unit.Id,

                            TransactionTypeId = chargeType.Id,
                            TransactionCategoryId = utilityCategory.Id,

                            //UtilityBillId = item.UtilityBillId,

                            Amount = item.Amount,
                            MonthFor = dto.MonthFor,
                            YearFor = dto.YearFor,
                            TransactionDate = dto.InvoiceDate,
                            Notes = "Utility charge"
                        };

                        entities.Add(entity);
                    }
                }

                await _repo.AddRangeAsync(entities);

                return ApiResponse<bool>.SuccessResponse(true, "Utility invoices generated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse(
                    ex.InnerException?.Message ?? ex.Message);
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



        public async Task<ApiResponse<List<TenantBalanceDto>>> GetUserBalances(int tenantId)
        {
            try
            {
                var domainUserId = await _tenantrepo.GetUserIdByTenantIdAsync(tenantId);

                if (domainUserId == null || !domainUserId.HasValue)
                    return ApiResponse<List<TenantBalanceDto>>.FailResponse("User ID not found.");

                var balances = await _repo.GetBalanceByUserAsync(domainUserId.Value);

                if (!balances.Any())
                    return new ApiResponse<List<TenantBalanceDto>>(null, "Tenant has no balance.");


                return new ApiResponse<List<TenantBalanceDto>>(balances, "Balances retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TenantBalanceDto>>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }








        // HELPER METHODS





        private List<CREATEIncoiceTransactionDto> CreateRentBalanceInvoice(
            List<Tenant> tenants,
            List<Transaction> existingCharges,
            int rentCategory,
            DateTime now)
        {
            var chargesLookup = existingCharges
                .Where(c => c.UserId.HasValue)
                .GroupBy(c => c.UserId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var invoices = new List<CREATEIncoiceTransactionDto>();

            foreach (var tenant in tenants)
            {
                if (tenant.User == null || tenant.Unit == null)
                    continue;

                var tenantId = tenant.User.Id;
                var monthlyRent = tenant.Unit.Amount;

                chargesLookup.TryGetValue(tenantId, out var tenantCharges);

                var totalCharged = tenantCharges?.Sum(c => c.Amount) ?? 0m;
                var balance = monthlyRent - totalCharged;

                if (balance <= 0)
                    continue;

                invoices.Add(new CREATEIncoiceTransactionDto
                {
                    UserId = tenantId,
                    MonthFor = now.Month,
                    YearFor = now.Year,
                    InvoiceDate = now,
                    Item = new List<CREATEItemDto>
            {
                new CREATEItemDto
                {
                    TransactionCategoryId = rentCategory,
                    Amount = balance
                }
            }
                });
            }

            return invoices;
        }


        private List<CREATEIncoiceTransactionDto> CreateFullRentInvoice(
            List<Tenant> tenants,
            int categoryId,
            DateTime now)
        {
            return tenants
                .Where(t => t.User != null && t.Unit != null)
                .Select(t => new CREATEIncoiceTransactionDto
                {
                    UserId = t.User.Id,
                    MonthFor = now.Month,
                    YearFor = now.Year,
                    InvoiceDate = now,
                    Item = new List<CREATEItemDto>
                    {
                new CREATEItemDto
                {
                    TransactionCategoryId = categoryId,
                    Amount = t.Unit.Amount
                }
                    }
                })
                .ToList();
        }


        private List<CREATEIncoiceTransactionDto> CreateFullUtilityInvoices(
            List<Tenant> tenants,
            List<UtilityBill> utilities,
            DateTime now)
        {
            var invoices = new List<CREATEIncoiceTransactionDto>();

            foreach (var tenant in tenants)
            {
                if (tenant.User == null || tenant.Unit == null)
                    continue;

                invoices.Add(new CREATEIncoiceTransactionDto
                {
                    UserId = tenant.User.Id,
                    MonthFor = now.Month,
                    YearFor = now.Year,
                    InvoiceDate = now,
                    Item = utilities.Select(u => new CREATEItemDto
                    {
                        UtilityBillId = u.Id,
                        Amount = u.Amount
                    }).ToList()
                });
            }

            return invoices;
        }


        private List<CREATEIncoiceTransactionDto> CreateUtilityBalanceInvoices(
            List<Tenant> tenants,
            List<UtilityBill> utilities,
            List<Transaction> existingCharges,
            DateTime now)
        {
            var invoices = new List<CREATEIncoiceTransactionDto>();

            // { UserId -> UtilityBillIds already billed }
            var billedLookup = existingCharges
                .Where(c => c.UserId.HasValue && c.UnitId.HasValue) // changed from utilitybill
                .GroupBy(c => c.UserId!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.UnitId!.Value).ToHashSet() // changed from utilitybill
                );

            foreach (var tenant in tenants)
            {
                if (tenant.User == null || tenant.Unit == null)
                    continue;

                billedLookup.TryGetValue(tenant.User.Id, out var billedUtilities);

                var items = utilities
                    .Where(u => billedUtilities == null || !billedUtilities.Contains(u.Id))
                    .Select(u => new CREATEItemDto
                    {
                        UtilityBillId = u.Id,
                        Amount = u.Amount
                    })
                    .ToList();

                if (!items.Any())
                    continue;

                invoices.Add(new CREATEIncoiceTransactionDto
                {
                    UserId = tenant.User.Id,
                    MonthFor = now.Month,
                    YearFor = now.Year,
                    InvoiceDate = now,
                    Item = items
                });
            }

            return invoices;
        }



        private Transaction CreateAdvancePaymentTransaction(CREATEPaymentDto payment, Tenant tenant, int transactionTypeId, int transactionCategoryId)
        {

            return new Transaction
            {
                UserId = tenant.User.Id,
                AccountId = _currentuser.AccountId,
                PropertyId = tenant.User.PropertyId,
                UnitId = tenant.Unit.Id,
                TransactionDate = DateTime.UtcNow,
                MonthFor = DateTime.UtcNow.Month,
                YearFor = DateTime.UtcNow.Year,
                Amount = payment.Amount,
                TransactionTypeId = transactionTypeId,
                TransactionCategoryId = transactionCategoryId,
                Notes = "Advance payment"
            };
        }


        

        private async Task<List<Transaction>> CreatePaymentTransactions(
            List<TenantBalanceDto> balances,
            Tenant tenant,
            decimal amountPaid,
            int paymentTypeId)
        {
            var newTransactions = new List<Transaction>();

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
                    AccountId = _currentuser.AccountId,
                    PropertyId = tenant.User.PropertyId,
                    UnitId = tenant.UnitId,
                    TransactionDate = DateTime.UtcNow,
                    MonthFor = bal.Month,
                    YearFor = bal.Year,
                    Amount = payable,
                    TransactionTypeId = paymentTypeId,
                    TransactionCategoryId = bal.CategoryId, 
                    //UtilityBillId = bal.UtilityBillId,
                    Notes = "Payment recorded"
                };

                newTransactions.Add(tx);
            }

            // If still money left → create advance payment
            if (amountPaid > 0)
            {
                var advanceCategory = await _systemcodeitemrepo.GetByCodeAndItemAsync("advance");

                var advanceTx = CreateAdvancePaymentTransaction(
                    new CREATEPaymentDto
                    {
                        Amount = amountPaid,
                        TenantId = tenant.Id
                    },
                    tenant,
                    paymentTypeId,
                    advanceCategory.Id
                );

                newTransactions.Add(advanceTx);
            }

            return newTransactions;
        }

    }
}
