using Microsoft.AspNetCore.Identity;
using RentalManager.Constants;
using RentalManager.DTOs.Commons;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.AuditTrailRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepo;
        private readonly IUtilityBillRepository _utilityRepo;
        private readonly ISystemCodeItemRepository _systemCodeItemRepo;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAuditTrailRepository _auditRepo;
        private readonly ICurrentUser _currentuser;


        public PropertyService(
            IPropertyRepository propertyRepo,
            IUtilityBillRepository utilityRepo,
            ISystemCodeItemRepository systemCodeItemRepo,
            IAuditTrailRepository auditRepo,
            UserManager<ApplicationUser> usermanager,
            ICurrentUser currentuser
            )
        {
            _propertyRepo = propertyRepo;
            _utilityRepo = utilityRepo;
            _systemCodeItemRepo = systemCodeItemRepo;
            _auditRepo = auditRepo;
            _usermanager = usermanager;
            _currentuser = currentuser;
        }



        public async Task<ApiResponse<List<READPropertyDto>?>> GetAll()
        {
            try
            {
                var properties = await _propertyRepo.GetAllAsync();

                if(properties == null || !properties.Any()) 
                    return ApiResponse<List<READPropertyDto>?>.SuccessResponse(new List<READPropertyDto>(), "No Properties Available");

                var propertyDtos = properties.Select(p => p.ToReadDto()).ToList();

                return ApiResponse<List<READPropertyDto>?>.SuccessResponse(propertyDtos, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<READPropertyDto>?>.FailResponse($"error occured: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<List<READPropertyLookupDto>?>> GetAllLookups()
        {
            try
            {
                var properties = await _propertyRepo.GetAllLookupAsync();

                if (properties == null || !properties.Any())
                    return ApiResponse<List<READPropertyLookupDto>?>.SuccessResponse(new List<READPropertyLookupDto>(), "No Properties Available");

                return ApiResponse<List<READPropertyLookupDto>?>.SuccessResponse(properties, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<READPropertyLookupDto>?>.FailResponse($"error occured: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<PagedResponse<List<READPropertyDto>>>> GetFiltered(PropertyQueryFilter filter)
        {
            try
            {
                var (data, total) = await _propertyRepo.GetFilteredAsync(filter);

                var dtos = data.Select(x => x.ToReadDto()).ToList();

                var paged = new PagedResponse<List<READPropertyDto>>(
                    dtos,
                    total,
                    filter.PageNumber,
                    filter.PageSize
                );

                return ApiResponse<PagedResponse<List<READPropertyDto>>>
                    .SuccessResponse(paged, "Fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<List<READPropertyDto>>>
                    .FailResponse($"An Error Occured, Please Try Again Later {ex.Message}");
            }
        }


        public async Task<ApiResponse<READPropertyDto?>> GetById(int id)
        {
            try
            {
                var property = await _propertyRepo.GetByIdAsync(id);

                if (property == null) return ApiResponse<READPropertyDto?>.SuccessResponse(new READPropertyDto(), "Items Not Found");

                var propertyDto = property.ToReadDto();

                return ApiResponse<READPropertyDto?>.SuccessResponse(propertyDto, "Property Fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<READPropertyDto?>.FailResponse($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto)
        {
            try
            {
                if (_currentuser.AccountId <= 0)
                    return ApiResponse<READPropertyDto>.FailResponse("Invalid account");

                var propertyTypeValid = await _systemCodeItemRepo.ExistsByIdAndCodeAsync(dto.PropertyTypeId, SystemCodeNames.Code.PropertyType);

                if (!propertyTypeValid)
                    return ApiResponse<READPropertyDto>.FailResponse("Invalid property type");

                var utilityValidation = await ValidateUtilities(dto.Utilities);

                if (!utilityValidation.Success)
                    return ApiResponse<READPropertyDto>.FailResponse(utilityValidation.Message);


                var property = dto.ToEntity(_currentuser.AccountId);
                var savedProperty = await _propertyRepo.AddProperty(property);
                var propertyType = await _systemCodeItemRepo.GetByIdAsync(dto.PropertyTypeId);
                var newValues = savedProperty.ToReadDto();
                newValues.PropertyTypeName = propertyType.Item;
                await AddAudit(savedProperty.Id, "CREATE", null, newValues);

                await SaveUtilities(savedProperty.Id, dto.Utilities);

                return ApiResponse<READPropertyDto>.SuccessResponse(newValues,"Property created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<READPropertyDto>.FailResponse(
                    $"An error occurred: {ex.Message}"
                );
            }
        }


        public async Task<ApiResponse<READPropertyDto>> Update(int id, PATCHPropertyDto dto)
        {
            try
            {
                var existingProperty = await _propertyRepo.GetByIdAsync(id);

                if (existingProperty == null)
                    return ApiResponse<READPropertyDto>
                        .FailResponse("Property does not exist");

                // 1. Get changes
                var changeResult = ObjectComparer.GetChanges(dto, existingProperty);

                if (!changeResult.HasChanges)
                {
                    return ApiResponse<READPropertyDto>
                        .SuccessResponse(existingProperty.ToReadDto(), "No changes detected");
                }
                // update entity
                ObjectComparer.ApplyChanges(dto, existingProperty);
                await _propertyRepo.UpdateAsync(existingProperty);

                // log audit
                var (oldValues, newValues) = ObjectComparer.SplitChanges(changeResult);
                await AddAudit(existingProperty.Id, "PATCH", oldValues, newValues);

                return ApiResponse<READPropertyDto>
                    .SuccessResponse(existingProperty.ToReadDto(), "Updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READPropertyDto>($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<READPropertyDto>> Delete(int id)
        {
            try
            {
                var property = await _propertyRepo.FindAsync(id);

                if (property == null)
                {
                    return ApiResponse<READPropertyDto>.SuccessResponse(null, "Property Does Not Exist");
                }

                await _propertyRepo.DeleteAsync(property);


                await AddAudit(property.Id, "DELETE", property, null);
                return ApiResponse<READPropertyDto>.SuccessResponse(null, "Deleted Successfully");
            }
            catch(Exception ex)
            {
                return ApiResponse<READPropertyDto>.FailResponse($"Error Occurred: { ex.Message } ");
            }
        }


        private async Task AddAudit(int entityId, string action, object? oldValues, object? newValues) 
        {
            var audit = new AuditTrail
            {
                AccountId = _currentuser.AccountId,
                UserId = _currentuser.UserId,
                EntityName = nameof(Property),
                EntityId = entityId,
                Action = action,
                OldValues = oldValues != null ? Serializer.SerializeSafely(oldValues) : null,
                NewValues = newValues != null ? Serializer.SerializeSafely(newValues) : null,
                CreatedAt = DateTime.UtcNow
            };
            await _auditRepo.AddAsync(audit);
        }


        private async Task<(bool Success, string Message)> ValidateUtilities(
            List<CREATEPropertyUtilityDto>? utilities)
        {
            if (utilities == null || !utilities.Any())
                return (true, string.Empty);

            var utilityIds = utilities
                .Select(x => x.UtilityId)
                .Distinct()
                .ToList();

            var billingCycleIds = utilities
                .Select(x => x.BillingCycleId)
                .Distinct()
                .ToList();

            var validUtilityIds = await _systemCodeItemRepo
                .AllExistByIdsAndCodeAsync(
                    utilityIds,
                    SystemCodeNames.Code.UtilityBill);

            var validBillingCycle = await _systemCodeItemRepo
                .AllExistByIdsAndCodeAsync(
                    billingCycleIds,
                    SystemCodeNames.Code.BillingCycle);

            if (!validUtilityIds || !validBillingCycle)
            {
                return (false, "One or more utilities are invalid");
            }

            return (true, string.Empty);
        }


        private async Task SaveUtilities(
            int propertyId,
            List<CREATEPropertyUtilityDto>? utilitiesDto)
        {
            if (utilitiesDto == null || !utilitiesDto.Any())
                return;

            // 1. Create and save entities
            var utilities = utilitiesDto.Select(u => new UtilityBill
            {
                PropertyId = propertyId,
                UtilityId = u.UtilityId,
                BillingCycleId = u.BillingCycleId,
                Amount = u.Amount,
                IsMetered = u.IsMetered,
                AccountId = _currentuser.AccountId
            }).ToList();

            await _utilityRepo.AddRangeAsync(utilities);

            // 2. Get all utility names in ONE query 
            var utilityIds = utilities.Select(x => x.UtilityId).ToList();

            var utilityItems = await _systemCodeItemRepo
                .GetByIdsAndCodeAsync(
                    utilityIds,
                    SystemCodeNames.Code.UtilityBill);

            var utilityLookup = utilityItems.ToDictionary(x => x.Id);

            // 3. Build audit records safely
            foreach (var utility in utilities)
            {
                if (!utilityLookup.TryGetValue(utility.UtilityId, out var utilityItem))
                    continue;

                var newValues = new READUtilityBillDto
                {
                    Id = utility.Id,
                    Name = utilityItem.Item,
                    Amount = utility.Amount,
                    IsMetered = utility.IsMetered,
                    PropertyId = propertyId,
                    BillingCycleId = utility.BillingCycleId,
                };

                await AddAudit(propertyId, "CREATE", null, newValues);
            }
        }
    }
}
