using RentalManager.Data;
using RentalManager.DTOs.Tenant;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
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
        private readonly ISystemCodeItemRepository _systemcoderepo;

        public TenantService(
            ApplicationDbContext context,
            ITenantRepository repo,
            IUserRepository userrepo,
            IRoleRepository rolerepo,
            IPropertyRepository propertyrepo,
            IUnitRepository unitrepo,
            ISystemCodeItemRepository systemcoderepo)
        {
            _context = context;
            _repo = repo;
            _userrepo = userrepo;
            _rolerepo = rolerepo;
            _propertyrepo = propertyrepo;
            _unitrepo = unitrepo;
            _systemcoderepo = systemcoderepo;
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
                var gender = await _systemcoderepo.FindAsync(tenant.User.GenderId);


                if (property == null || gender == null)
                {
                    return new ApiResponse<READTenantDto>(null, "One of the items provided does not exist");
                }


                var defaultRole = await _rolerepo.GetByNameAsync("Tenant");
                var defaultUserStatus = await _systemcoderepo.GetByItemAsync("Active");
                var defaultStatus = await _systemcoderepo.GetByItemAsync("Pending");

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


                var gender = await _systemcoderepo.FindAsync(tenant.User.GenderId);
                var userStatus= await _systemcoderepo.FindAsync(tenant.User.UserStatusId);
                var status = await _systemcoderepo.FindAsync(tenant.Status);


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


                if (assignedTenant == null || assignedUnit == null)
                {
                    return new ApiResponse<READTenantDto>(null, "One of the items provided does not exist");
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
