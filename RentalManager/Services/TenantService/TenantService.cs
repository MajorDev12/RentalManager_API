using RentalManager.Data;
using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UserRepository;

namespace RentalManager.Services.TenantService
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantRepository _repo;
        private readonly IUserRepository _userrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IRoleRepository _rolerepo;
        private readonly IUnitRepository _unitrepo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly ITransactionRepository _transactionrepo;

        public TenantService(
            ApplicationDbContext context,
            ITenantRepository repo,
            IUserRepository userrepo,
            IRoleRepository rolerepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ITransactionRepository transactionRepository)
        {
            _context = context;
            _repo = repo;
            _userrepo = userrepo;
            _rolerepo = rolerepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _transactionrepo = transactionRepository;
        }


        public async Task<ApiResponse<List<READTenantDto>>> GetAll()
        {
            try
            {
                var tenants = await _repo.GetAllAsync();

                if (tenants == null || tenants.Count == 0)
                {
                    return new ApiResponse<List<READTenantDto>>(null, "Data Not Found.");
                }

                var tenantDtos = tenants.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READTenantDto>>(tenantDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READTenantDto>>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> GetById(int id)
        {
            try
            {
                var tenant = await _repo.GetByIdAsync(id);

                if (tenant == null)
                {
                    return new ApiResponse<READTenantDto>(null, "Data Not Found.");
                }

                var tenantDtos = tenant.ToReadDto();

                return new ApiResponse<READTenantDto>(tenantDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex.InnerException?.Message}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> Add(CREATETenantDto tenant)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var property = await _propertyrepo.FindAsync(tenant.User.PropertyId);
                var gender = await _systemcodeitemrepo.FindAsync(tenant.User.GenderId);


                if (property == null || gender == null)
                {
                    return new ApiResponse<READTenantDto>(null, "One of the items provided does not exist");
                }


                var defaultRole = await _rolerepo.GetByNameAsync("Tenant");
                var defaultUserStatus = await _systemcodeitemrepo.GetByItemAsync("Active");
                var defaultStatus = await _systemcodeitemrepo.GetByItemAsync("Pending");

                if (defaultRole == null || defaultStatus == null || defaultUserStatus == null)
                {
                    return new ApiResponse<READTenantDto>("Missing default role, TenantStatus or UserStatus");
                }


                // Create User
                var user = tenant.User.ToEntity(defaultRole.Id, defaultUserStatus.Id);
                var addedUser = await _userrepo.AddAsync(user);

                // Create Tenant
                var entity = tenant.ToEntity(addedUser, defaultStatus.Id);
                var addedTenant= await _repo.AddAsync(entity);

                if (addedUser == null || addedTenant == null)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<READTenantDto>(null, "Data Not Found.");
                }

                // Commit Transaction
                await transaction.CommitAsync();

                return new ApiResponse<READTenantDto>(null, "Tenant Created Successfully");
            }
            catch (Exception ex)
            {
                // Rollback
                await transaction.RollbackAsync();
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> Update(int tenantId, UPDATETenantDto tenant)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var existing = await _repo.FindAsync(tenantId);

                if (existing == null) return new ApiResponse<READTenantDto>(null, "No Such Data.");


                var gender = await _systemcodeitemrepo.FindAsync(tenant.User.GenderId);
                var userStatus= await _systemcodeitemrepo.FindAsync(tenant.User.UserStatusId);
                var status = await _systemcodeitemrepo.FindAsync(tenant.Status);


                if (status == null || gender == null)
                {
                    return new ApiResponse<READTenantDto>(null, "One of the items provided does not exist: status, gender.");
                }

                // Update User
                var userEntity = tenant.User.ToEntity(existing.UserId);
                var updatedUser = await _userrepo.UpdateAsync(userEntity);

                // Update Tenant
                var tenantEntity = tenant.ToEntity(updatedUser, status.Id, existing.Id);
                var updatedTenant = await _repo.UpdateAsync(tenantEntity, updatedUser);

                if (updatedUser == null || updatedTenant == null)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<READTenantDto>(null, "Data Not Found.");
                }

                await transaction.CommitAsync();

                return new ApiResponse<READTenantDto>(null, "Updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> Delete(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _repo.GetByIdAsync(id);

                if (entity == null)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<READTenantDto>("Data Not Found.");
                }

                await _repo.DeleteAsync(entity);
                await _userrepo.DeleteAsync(entity.User);

                await transaction.CommitAsync();

                return new ApiResponse<READTenantDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> AssignUnit(ASSIGNUnitDto unitAssigned)
        {
            try
            {
                var assignedTenant = await _repo.GetByIdAsync(unitAssigned.tenantId);
                var assignedUnit = await _unitrepo.GetByIdAsync(unitAssigned.unitId);
                var tenantStatus = await _systemcodeitemrepo.GetByIdAsync(unitAssigned.status);
                

                if (assignedTenant == null || assignedUnit == null || tenantStatus == null)
                {
                    return new ApiResponse<READTenantDto>(null, "One of the items provided does not exist");
                }

                    // check if already occupied
                if (assignedUnit.Status != null && String.Equals(assignedUnit.Status.Item, "occupied", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<READTenantDto>(null, "unit assigned is already occupied");
                }

                var property = await _propertyrepo.FindAsync(assignedTenant.User.PropertyId);

                if (property == null)
                {
                    return new ApiResponse<READTenantDto>(null, "Tenant Property not found.");
                }

                if (property.Id != assignedUnit.PropertyId)
                {
                    return new ApiResponse<READTenantDto>(null, "Unit doesn't match tenant property.");
                }


                var entity = unitAssigned.ToEntity();
                var assigned = await _repo.AssignUnitAsync(entity);

                if (tenantStatus.Item?.ToLower() == "active")
                {

                    var deposit = await _systemcodeitemrepo.GetByItemAsync("deposit");
                    var rent = await _systemcodeitemrepo.GetByItemAsync("rent");
                    var charge = await _systemcodeitemrepo.GetByItemAsync("charge");
                    var payment = await _systemcodeitemrepo.GetByItemAsync("payment");

                    if (deposit == null || charge == null || rent == null || payment == null) 
                        return new ApiResponse<READTenantDto>(null, "Something went wrong with payments.");



                    // Rent charge
                    var rentCharge = new CREATETransactionDto
                    {
                        UserId = assignedTenant.UserId,
                        UnitId = assignedUnit.Id,
                        TransactionTypeId = charge.Id,
                        TransactionCategoryId = rent.Id,
                        Amount = assignedUnit.Amount,
                        TransactionDate = unitAssigned.PaymentDate,
                        MonthFor = DateTime.UtcNow.Month,
                        YearFor = DateTime.UtcNow.Year,
                        Notes = "Initial rent charge"
                    };

                    var rentChargeEntity = rentCharge.ToEntity();
                    var addedChargeRent = await _transactionrepo.AddAsync(rentChargeEntity);


                    // Deposit payment
                    var depositTransaction = new CREATETransactionDto
                    {
                        UserId = assignedTenant.UserId,
                        UnitId = assignedUnit.Id,
                        TransactionTypeId = payment.Id,
                        TransactionCategoryId = deposit.Id,
                        Amount = unitAssigned.DepositAmount,
                        TransactionDate = unitAssigned.PaymentDate,
                        MonthFor = DateTime.UtcNow.Month,
                        YearFor = DateTime.UtcNow.Year,
                        Notes = "Initial deposit charge"
                    };

                    var depositEntity = depositTransaction.ToEntity();
                    await _transactionrepo.AddAsync(depositEntity);



                    //rent payment
                    var rentPayment = new CREATETransactionDto
                    {
                        UserId = assignedTenant.UserId,
                        UnitId = assignedUnit.Id,
                        TransactionTypeId = payment.Id,
                        TransactionCategoryId = rent.Id,
                        Amount = unitAssigned.AmountPaid,
                        TransactionDate = unitAssigned.PaymentDate,
                        MonthFor = DateTime.UtcNow.Month,
                        YearFor = DateTime.UtcNow.Year,
                        Notes = "Initial rent charge"
                    };

                    if (addedChargeRent != null)
                    {
                        var rentPaymentEntity = rentPayment.ToEntity();
                        await _transactionrepo.AddAsync(rentPaymentEntity);

                        //update house status to occupied
                        var status = await _systemcodeitemrepo.GetByItemAsync("occupied");

                        if (status != null && status.SystemCode.Code.ToLower() == "unitstatus")
                        {
                            var statusUpdated = assignedUnit.UpdateStatusEntity(status.Id);
                            var statusSaved = await _unitrepo.UpdateStatus();

                            if(statusSaved <= 0){
                                return new ApiResponse<READTenantDto>(null, "failed to change Unit Status");
                            }
                        }
                        else
                        {
                            return new ApiResponse<READTenantDto>(null, "failed to change Unit Status");
                        }
                    }


                }



                if (assigned == null)
                {
                    return new ApiResponse<READTenantDto>(null, "Data Not Found.");
                }

                return new ApiResponse<READTenantDto>(null, "Tenant Assigned Unit Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex}");
            }
        }
    }
}
