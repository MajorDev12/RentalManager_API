using RentalManager.BusinessRules.Core;
using RentalManager.Constants;
using RentalManager.DTOs.Commons;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.AuditTrailRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.UtilityBillService
{
    public class UtilityBillService : IUtilityBillService
    {

        private readonly IUtilityBillRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly ITenantRepository _tenantrepo;
        private readonly IUnitRepository _unitRepo;
        private readonly ISystemCodeItemRepository _systemCodeItemRepo;
        private readonly IAuditTrailRepository _auditRepo;
        private readonly IRuleEngine _ruleEngine;
        private readonly ICurrentUser _currentuser;

        public UtilityBillService(
            IUtilityBillRepository repo,
            IPropertyRepository propertyrepo,
            ITenantRepository tenantrepo,
            IUnitRepository unitRepo,
            ISystemCodeItemRepository systemCodeItemRepo,
            IAuditTrailRepository auditRepo,
            IRuleEngine ruleEngine,
            ICurrentUser currentuser)
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _tenantrepo = tenantrepo;
            _unitRepo = unitRepo;
            _systemCodeItemRepo = systemCodeItemRepo;
            _auditRepo = auditRepo;
            _ruleEngine = ruleEngine;
            _currentuser = currentuser;
        }



        public async Task<ApiResponse<List<READUtilityBillDto>>> GetAll()
        {
            try
            {
                var bills = await _repo.GetAllAsync(_currentuser);

                if (bills == null)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Items Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<List<UtilityLookupDto>>> GetAllLookups()
        {
            try
            {
                var bills = await _repo.GetAllLookupsAsync();

                if (bills == null)
                {
                    return new ApiResponse<List<UtilityLookupDto>>(null, "Items Not Found.");
                }

                return new ApiResponse<List<UtilityLookupDto>>(bills, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<UtilityLookupDto>>($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<PagedResponse<List<READUtilityBillDto>>>> GetFiltered(UtilityBillQueryFilter filter)
        {
            try
            {
                var (bills, total) = await _repo.GetFilteredAsync(filter);

                if (bills == null)
                {
                    return ApiResponse<PagedResponse<List<READUtilityBillDto>>>.FailResponse("Items Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                var paged = new PagedResponse<List<READUtilityBillDto>>(
                    billDtos,
                    total,
                    filter.PageNumber,
                    filter.PageSize
                );

                return ApiResponse<PagedResponse<List<READUtilityBillDto>>>.SuccessResponse(paged, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<List<READUtilityBillDto>>>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> GetById(int id)
        {
            try
            {
                var bill = await _repo.GetByIdAsync(id);

                if (bill == null)
                {
                    return new ApiResponse<READUtilityBillDto>(null, "Items Not Found.");
                }

                var billDto = bill.ToReadDto();

                return new ApiResponse<READUtilityBillDto>(billDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<List<READUtilityBillDto>>> GetByPropertyId(int propertyId, bool? isMetered = null)
        {
            try
            {
                var bills = await _repo.GetByPropertyIdAsync(propertyId, isMetered);

                if (bills == null || bills.Count == 0)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Items Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<List<READUtilityBillDto>>> GetByUnitId(int unitId, bool? isMetered = null)
        {
            try
            {
                var bills = await _repo.GetUtilitiesByUnitAsync(unitId, isMetered);

                if (bills == null || bills.Count == 0)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Items Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>($"Error Occurred: {ex.InnerException}");
            }
        }


        public async Task<ApiResponse<List<READUtilityBillDto>>> GetByTenantId(int id)
        {
            try
            {
                var tenant = await _tenantrepo.GetByIdAsync(id);

                if(tenant == null)
                    return new ApiResponse<List<READUtilityBillDto>>(null, "tenant was not found");

                var bills = await _repo.GetByPropertyIdAsync(tenant.Unit.PropertyId);


                if (bills == null || bills.Count == 0)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Items Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> Add(CREATEUtilityBillDto AddedBill)
        {
            try
            {
                var property = await _propertyrepo.FindAsync(AddedBill.PropertyId);
                var billingCycle = await _systemCodeItemRepo.ExistsByIdAndCodeAsync(AddedBill.BillingCycleId, SystemCodeNames.Code.BillingCycle);

                if (property == null) return ApiResponse<READUtilityBillDto>.FailResponse("Property Provided Does Not Exist.");
                if (!billingCycle) return ApiResponse<READUtilityBillDto>.FailResponse("BillingCycle Provided Does Not Exist.");

                if (AddedBill.UnitId is not null)
                {
                    var unitExists = await _unitRepo.ExistsAsync(AddedBill.UnitId.Value);
                    var unitExistsInProperty = await _unitRepo.ExistsInPropertyAsync(AddedBill.UnitId.Value, AddedBill.PropertyId);

                    if(!unitExists || !unitExistsInProperty) return ApiResponse<READUtilityBillDto>.FailResponse("Unit Provided Does Not Exist or NotExist in Property selected.");
                }



                var entity = AddedBill.ToEntity();
                entity.AccountId = _currentuser.AccountId;

                await _ruleEngine.ValidateAsync(entity, RuleOperation.Create);
                var bill = await _repo.AddAsync(entity);

                if (bill == null)
                {
                    return ApiResponse<READUtilityBillDto>.FailResponse("Items Not Found.");
                }

                await AddAudit(bill.Id, "CREATE", null, bill);
                return ApiResponse<READUtilityBillDto>.SuccessResponse(null, "Utility Bill Created Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<READUtilityBillDto>.FailResponse($"Error Occurred: {ex.Message} ");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> Patch(int id, PATCHUtilityDto updatedUtilityDto)
        {
            //verify utility
            var existingUtility = await _repo.GetByIdAsync(id);


            if (existingUtility == null)
                return ApiResponse<READUtilityBillDto>
                    .FailResponse("Utility does not exist");

            var changeResult = ObjectComparer.GetChanges(updatedUtilityDto, existingUtility);

            if (!changeResult.HasChanges)
            {
                return ApiResponse<READUtilityBillDto>
                    .SuccessResponse(existingUtility.ToReadDto(), "No changes detected");
            }



            // verify property and utilityId
            if (updatedUtilityDto.PropertyId.HasValue)
            {
                var property = await _propertyrepo.GetByIdAsync(updatedUtilityDto.PropertyId.Value);

                if(property == null)
                    return ApiResponse<READUtilityBillDto>
                    .FailResponse("Property Selected Does Not Exist");
            }

            if (updatedUtilityDto.UtilityId.HasValue)
            {
                var utilityCode = await _systemCodeItemRepo.ExistsByIdAndCodeAsync(updatedUtilityDto.UtilityId.Value, SystemCodeNames.Code.UtilityBill);
               
                if (!utilityCode)
                    return ApiResponse<READUtilityBillDto>
                    .FailResponse("Utility Selected Does Not Exist");
            }

            if (updatedUtilityDto.BillingCycleId.HasValue)
            {
                var billingCycle = await _systemCodeItemRepo.ExistsByIdAndCodeAsync(updatedUtilityDto.BillingCycleId.Value, SystemCodeNames.Code.BillingCycle);

                if (!billingCycle)
                    return ApiResponse<READUtilityBillDto>
                    .FailResponse("billingCycle Selected Does Not Exist");
            }

            if (updatedUtilityDto.UnitId.HasValue)
            {
                var unitExist = _unitRepo.ExistsAsync(updatedUtilityDto.UnitId.Value);
                var unitExistInProperty = _unitRepo.ExistsInPropertyAsync(updatedUtilityDto.UnitId.Value, updatedUtilityDto.UnitId.Value);
            }


            await _ruleEngine.ValidateAsync(updatedUtilityDto, RuleOperation.Update);
            

            // make changes
            ObjectComparer.ApplyChanges(updatedUtilityDto, existingUtility);
            await _repo.UpdateAsync(existingUtility);


            // log audit
            var (oldValues, newValues) = ObjectComparer.SplitChanges(changeResult);
            await AddAudit(existingUtility.Id, "PATCH", oldValues, newValues);

            return ApiResponse<READUtilityBillDto>
                .SuccessResponse(null, "Updated successfully");

        }

        public async Task<ApiResponse<READUtilityBillDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READUtilityBillDto>("Items Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUtilityBillDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>($"Error Occurred: {ex.Message}");
            }
        }


        private async Task AddAudit(int entityId, string action, object? oldValues, object? newValues)
        {
            var audit = new AuditTrail
            {
                AccountId = _currentuser.AccountId,
                UserId = _currentuser.UserId,
                EntityName = nameof(UtilityBill),
                EntityId = entityId,
                Action = action,
                OldValues = oldValues != null ? Serializer.SerializeSafely(oldValues) : null,
                NewValues = newValues != null ? Serializer.SerializeSafely(newValues) : null,
                CreatedAt = DateTime.UtcNow
            };
            await _auditRepo.AddAsync(audit);
        }



    }
}
