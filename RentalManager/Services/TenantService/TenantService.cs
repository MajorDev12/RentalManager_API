using RentalManager.Data;
using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Transaction;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Notification.Events;
using RentalManager.Notification.Services;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Services.AccountAccessService;
using RentalManager.Services.UserService;
using static RentalManager.Authorization.Policies.PolicyNames;

namespace RentalManager.Services.TenantService
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantRepository _repo;
        private readonly IUserService _userservice;
        private readonly IUserRepository _userrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IRoleRepository _rolerepo;
        private readonly IUnitRepository _unitrepo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly ITransactionRepository _transactionrepo;
        private readonly ICurrentUser _currentuser;
        private readonly NotificationPreferenceSeeder _notificationPreferenceSeeder;
        private readonly NotificationService _notificationService;

        public TenantService(
            ApplicationDbContext context,
            ITenantRepository repo,
            IUserRepository userrepo,
            IUserService userservice,
            IRoleRepository rolerepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ITransactionRepository transactionRepository,
            ICurrentUser currentuser,
            NotificationPreferenceSeeder notificationPreferenceSeeder,
            NotificationService notificationService)
        {
            _context = context;
            _repo = repo;
            _userrepo = userrepo;
            _userservice = userservice;
            _rolerepo = rolerepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _transactionrepo = transactionRepository;
            _currentuser = currentuser;
            _notificationPreferenceSeeder = notificationPreferenceSeeder;
            _notificationService = notificationService;
        }


        public async Task<ApiResponse<List<READTenantDto>>> GetAll()
        {
            try
            {
                var tenants = await _repo.GetAllAsync();

                if (tenants == null || tenants.Count == 0)
                {
                    return new ApiResponse<List<READTenantDto>>(null, "Items Not Found.");
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
                    return new ApiResponse<READTenantDto>(null, "Items Not Found.");
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
                    return ApiResponse<READTenantDto>.FailResponse("One of the items provided does not exist");
                }


                var defaultRole = await _rolerepo.GetByNameAsync("Tenant");
                var defaultUserStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync("Active");
                var defaultStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync("Pending");

                if (defaultRole == null || defaultStatus == null || defaultUserStatus == null)
                {
                    return ApiResponse<READTenantDto>.FailResponse("Missing default role, TenantStatus or UserStatus");
                }


                // create AppUser and Domain User
                tenant.User.RoleId = defaultRole.Id;
                tenant.User.UserStatusId = defaultStatus.Id;
                tenant.Status = defaultStatus.Id;
                var result = await _userservice.Add(tenant.ToUserDto());

                if (!result.Success || result.Data == null) return new ApiResponse<READTenantDto>(null, result.Message, false);


                // Create Tenant
                var entity = tenant.ToEntity(result.Data, defaultStatus.Id);
                entity.AccountId = _currentuser.AccountId;
                var addedTenant= await _repo.AddAsync(entity);

                if (addedTenant == null)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<READTenantDto>.FailResponse("Something went wrong while saving data.");
                }

                

                var tenantCreated = await _repo.GetByIdAsync(addedTenant.Id);

                if(tenantCreated == null)
                    return ApiResponse<READTenantDto>.FailResponse("Error: Failed to add tenant");

                await _notificationPreferenceSeeder.SeedForUserAsync(
                    accountId: tenantCreated.AccountId,
                    userId: tenantCreated.UserId,
                    role: tenantCreated.User.Account.Name);


                await _notificationService.NotifyAsync(
                    new TenantAddedEvent
                    {
                        AccountId = tenantCreated.AccountId,
                        PropertyId = tenantCreated.User.AccountId,
                        TenantUserId = tenantCreated.UserId
                    });

                // Commit Transaction
                await transaction.CommitAsync();

                return ApiResponse<READTenantDto>.SuccessResponse(tenantCreated.ToReadDto(), "Tenant Created Successfully");
            }
            catch (Exception ex)
            {
                // Rollback
                await transaction.RollbackAsync();
                return ApiResponse<READTenantDto>.FailResponse($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<READTenantDto>> Update(int tenantId, UPDATETenantDto tenant)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var existing = await _repo.FindAsync(tenantId);

                if (existing == null) return ApiResponse<READTenantDto>.FailResponse("Tenant Does Not Exist.");

                SystemCodeItem? gender = new SystemCodeItem();

                if (tenant.User.GenderId > 0)
                    gender = await _systemcodeitemrepo.FindAsync(tenant.User.GenderId);


                if (gender == null)
                {
                    return ApiResponse<READTenantDto>.FailResponse("One of the items provided does not exist, gender.");
                }

                // Update User
                var userEntity = tenant.User.ToEntity();
                var updatedUser = await _userrepo.UpdateAsync(userEntity);

                // Update Tenant
                //var tenantEntity = tenant.ToEntity(updatedUser);
                var updatedTenant = await _repo.UpdateAsync();

                if (updatedUser == null || updatedTenant == null)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<READTenantDto>.FailResponse("Items Not Found.");
                }

                await transaction.CommitAsync();

                return ApiResponse<READTenantDto>.SuccessResponse(null, "Updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<READTenantDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
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
                    return new ApiResponse<READTenantDto>("Items Not Found.");
                }
                var currentDomainUserId = await _userrepo.GetIdByApplicationUserIdAsync(_currentuser.UserId);

                await SoftDeleteUserAsync(entity.User.Id, currentDomainUserId);


                //change unit to vacant
                if (entity.UnitId.HasValue)
                {
                    var unitStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync("Vacant", "UNITSTATUS");

                    if (unitStatus == null)
                        return new ApiResponse<READTenantDto>($"Unit Status {unitStatus} Does Not exist but Tenant is deleted");

                    await _unitrepo.UpdateStatus(entity.UnitId.Value, unitStatus.Id);
                }

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
            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var tenant = await _repo.GetByIdAsync(unitAssigned.tenantId);
                var unit = await _unitrepo.GetByIdAsync(unitAssigned.unitId);
                var newStatus = await _systemcodeitemrepo.GetByIdAsync(unitAssigned.statusId);

                if (tenant == null || unit == null || newStatus == null)
                    return ApiResponse<READTenantDto>.FailResponse("Invalid tenant, unit, or status.");

                if (newStatus.Item != "active" && tenant.UnitId != null)
                    return ApiResponse<READTenantDto>.FailResponse("Tenant already has a unit.");

                if (unit.Status?.Item.Equals("occupied", StringComparison.OrdinalIgnoreCase) == true)
                    return ApiResponse<READTenantDto>.FailResponse("Unit is already occupied.");

                if (tenant.User.PropertyId != unit.PropertyId)
                    return ApiResponse<READTenantDto>.FailResponse("Unit does not belong to tenant property.");


                SystemCodeItem? rent = null, deposit = null, charge = null, payment = null, occupied = null;

                if (newStatus.Item.Equals("active", StringComparison.OrdinalIgnoreCase))
                {
                    await DetachUnitFromAllTenants(unitAssigned.unitId);

                    rent = await _systemcodeitemrepo.GetByCodeAndItemAsync("rent");
                    deposit = await _systemcodeitemrepo.GetByCodeAndItemAsync("deposit");
                    charge = await _systemcodeitemrepo.GetByCodeAndItemAsync("charge");
                    payment = await _systemcodeitemrepo.GetByCodeAndItemAsync("payment");
                    occupied = await _systemcodeitemrepo.GetByCodeAndItemAsync("occupied");

                    if (rent == null || deposit == null || charge == null || payment == null || occupied == null)
                        return ApiResponse<READTenantDto>.FailResponse("Required system codes missing.");

                }

                var tenantUpdated = await _repo.AssignUnitAsync(unitAssigned.ToEntity());


                if (newStatus.Item.Equals("active", StringComparison.OrdinalIgnoreCase))
                {
                    var now = DateTime.UtcNow;

                    var initialRentCharge = new CREATETransactionDto
                    {
                        UserId = tenant.UserId,
                        PropertyId = unit.PropertyId,
                        UnitId = unit.Id,
                        TransactionTypeId = charge!.Id,
                        TransactionCategoryId = rent!.Id,
                        Amount = unit.Amount,
                        TransactionDate = unitAssigned.PaymentDate ?? DateTime.UtcNow,
                        MonthFor = now.Month,
                        YearFor = now.Year,
                        Notes = "Initial rent charge"
                    }.ToEntity();

                    initialRentCharge.AccountId = _currentuser.AccountId;
                    await _transactionrepo.AddAsync(initialRentCharge);

                    var depositCharge = new CREATETransactionDto
                    {
                        UserId = tenant.UserId,
                        PropertyId = unit.PropertyId,
                        UnitId = unit.Id,
                        TransactionTypeId = payment!.Id,
                        TransactionCategoryId = deposit!.Id,
                        Amount = unitAssigned.DepositAmount,
                        TransactionDate = unitAssigned.PaymentDate ?? DateTime.UtcNow,
                        MonthFor = now.Month,
                        YearFor = now.Year,
                        Notes = "Deposit payment"
                    }.ToEntity();

                    depositCharge.AccountId = _currentuser.AccountId;
                    await _transactionrepo.AddAsync(depositCharge);

                    var rentPayment = new CREATETransactionDto
                    {
                        UserId = tenant.UserId,
                        PropertyId = unit.PropertyId,
                        UnitId = unit.Id,
                        TransactionTypeId = payment.Id,
                        TransactionCategoryId = rent.Id,
                        Amount = unitAssigned.AmountPaid,
                        TransactionDate = unitAssigned.PaymentDate ?? DateTime.UtcNow,
                        MonthFor = now.Month,
                        YearFor = now.Year,
                        Notes = "Initial rent payment"
                    }.ToEntity();

                    rentPayment.AccountId = _currentuser.AccountId;
                    await _transactionrepo.AddAsync(rentPayment);


                    await _unitrepo.UpdateStatus(unit.Id, occupied!.Id);

                    var updated = await _repo.UpdateTenantStatusAsync(tenantUpdated.Id, newStatus.Id);

                    if (updated == null)
                        return ApiResponse<READTenantDto>.FailResponse("Tenant not found");

                }


                await tx.CommitAsync();

                return ApiResponse<READTenantDto>.SuccessResponse(tenantUpdated?.ToReadDto(), "Tenant assigned successfully.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return ApiResponse<READTenantDto>.FailResponse(
                    $"Error Occured: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
        }



        public async Task<ApiResponse<READTenantDto>> AssignStatus(ASSIGNStatusDto statusDto)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            try 
            {
                var tenantStatus = await _systemcodeitemrepo.GetByIdAsync(statusDto.Status);
                var tenant = await _repo.GetByIdAsync(statusDto.TenantId);



                if (tenantStatus == null || tenant == null) return ApiResponse<READTenantDto>.FailResponse("One of the items provided does not exist");

                if (tenant.TenantStatusId == statusDto.Status) return ApiResponse<READTenantDto>.FailResponse("No Changes Made");

                var occupiedStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync("Occupied", "UNITSTATUS");
                var vacantStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync("Vacant", "UNITSTATUS");


                if(occupiedStatus == null || vacantStatus == null)
                    return ApiResponse<READTenantDto>.FailResponse("System Codes Error");


                if (tenantStatus.Item.ToLower() != "active" && tenant.UnitId.HasValue) 
                {
                    await _unitrepo.UpdateStatus(tenant.UnitId.Value, vacantStatus.Id);
                }

                await _repo.UpdateTenantStatusAsync(tenant.Id, tenantStatus.Id);

                await tx.CommitAsync();
                return ApiResponse<READTenantDto>.SuccessResponse(tenant.ToReadDto(), "Status Changed Successfuly");
            }
            catch(Exception ex)
            {
                await tx.RollbackAsync();
                return ApiResponse<READTenantDto>.FailResponse(ex.InnerException?.Message ?? ex.Message);
            }
        }



        private async Task SoftDeleteUserAsync(int userId, int currentUserId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return;

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;
            user.DeletedBy = currentUserId;

            await _context.SaveChangesAsync();
        }


        private async Task DetachUnitFromAllTenants(int unitId)
        {
            var tenants = await _repo.GetAllByUnitId(unitId);

            if (tenants == null || !tenants.Any())
                return;

            foreach (var tenant in tenants)
            {
                tenant.UnitId = null;
            }

            await _context.SaveChangesAsync();
        }

    }
}
