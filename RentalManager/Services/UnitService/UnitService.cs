using RentalManager.Constants;
using RentalManager.DTOs.Commons;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.AuditTrailRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UnitTypeRepository;
using RentalManager.Services.AccountAccessService;
using System.Data;

namespace RentalManager.Services.UnitService
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IUnitTypeRepository _unittyperepo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly ICurrentUser _currentuser;
        private readonly IAuditTrailRepository _auditRepo;

        public UnitService(
            IUnitRepository repo, 
            IPropertyRepository propertyrepo, 
            IUnitTypeRepository unittyperepo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ICurrentUser currentuser,
            IAuditTrailRepository auditRepo
            )
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _unittyperepo = unittyperepo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _currentuser = currentuser;
            _auditRepo = auditRepo;
        }

        public async Task<ApiResponse<List<READUnitDto>>> GetAll()
        {
            try
            {
                var units = await _repo.GetAllAsync();

                if (units == null)
                {
                    return new ApiResponse<List<READUnitDto>>(null, "Items Not Found.");
                }

                var unitDtos = units.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUnitDto>>(unitDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUnitDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<List<READUnitLookupDto>>> GetLookups()
        {
            try
            {
                var units = await _repo.GetLookupsAsync();

                if (units == null)
                {
                    return ApiResponse<List<READUnitLookupDto>>.FailResponse("Items Not Found, Try Inserting First.");
                }

                return ApiResponse<List<READUnitLookupDto>>.SuccessResponse(units, "Retrieved Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<READUnitLookupDto>>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<PagedResponse<List<READUnitDto>>>> GetFiltered(UnitQueryFilter filter)
        {
            try
            {
                var (data, total) = await _repo.GetFilteredAsync(filter);

                var dtos = data.Select(x => x.ToReadDto()).ToList();

                var paged = new PagedResponse<List<READUnitDto>>(
                    dtos,
                    total,
                    filter.PageNumber,
                    filter.PageSize
                );

                return ApiResponse<PagedResponse<List<READUnitDto>>>
                    .SuccessResponse(paged, "Fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<List<READUnitDto>>>
                    .FailResponse($"An Error Occured, Please Try Again Later {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<READUnitDto>> GetById(int id)
        {
            try
            {
                var unit = await _repo.GetByIdAsync(id);

                if (unit == null)
                {
                    return ApiResponse<READUnitDto>.FailResponse("Items Not Found.");
                }

                var codeDtos = unit.ToReadDto();

                return new ApiResponse<READUnitDto>(codeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<List<READUnitDto>>> GetByPropertyId(int id)
        {
            try
            {
                var units = await _repo.GetByPropertyIdAsync(id);

                if (units == null || units.Count == 0)
                {
                    return new ApiResponse<List<READUnitDto>>(null, "Items Not Found.");
                }

                var unitDtos = units.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUnitDto>>(unitDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUnitDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Add(CREATEUnitDto unit)
        {
            try
            {
                // check if property Exist
                var checker = await UnitChecker(unit);
                if (!checker.Item1) return ApiResponse<READUnitDto>.FailResponse(checker.Item2);

                // Get Configs
                var unitStatus = await _systemcodeitemrepo.GetByCodeAndItemAsync(SystemCodeNames.Item.UnitStatus.Vacant, SystemCodeNames.Code.UnitStatus);
                if (unitStatus == null) return ApiResponse<READUnitDto>.FailResponse("System Config Error, Try Again Later");


                var entity = unit.ToEntity(unitStatus.Id);
                entity.AccountId = _currentuser.AccountId;

                var types = await _repo.AddAsync(entity);

                if (types == null)
                {
                    return ApiResponse<READUnitDto>.FailResponse("Items Not Found.");
                }
                await AddAudit(types.Id, "CREATE", null, types);


                return ApiResponse<READUnitDto>.SuccessResponse(null, "Unit Created Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<READUnitDto>.FailResponse($"Error Occurred: {ex.InnerException?.Message} ");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Update(int id, UPDATEUnitDto unit)
        {
            try
            {

                var existing = await _repo.FindAsync(id);

                if (existing == null) return ApiResponse<READUnitDto>.FailResponse("No Such Items.");

                var entity = unit.ToEntity();


                var property = await _propertyrepo.FindAsync(unit.PropertyId);
                var unitType = await _unittyperepo.FindAsync(_currentuser, unit.UnitTypeId);

                if (property == null || unitType == null) 
                    return ApiResponse<READUnitDto>.FailResponse("One Of The Items Provided Does Not Exist.");


                var updatedEntity = entity.UpdateEntity(existing);
                await _repo.UpdateAsync(entity);


                return new ApiResponse<READUnitDto>(updatedEntity.ToReadDto(), "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Patch(int id, PATCHUnitDto dto)
        {
            try
            {
                var existingUnit = await _repo.GetByIdAsync(id);
                if(existingUnit is null) return ApiResponse<READUnitDto>.FailResponse("Unit Does Not Exist, Try Again Later");

                var checker = await UnitChecker(id, existingUnit.PropertyId, dto);
                if(!checker.Item1) return ApiResponse<READUnitDto>.FailResponse(checker.Item2);


                // 1. Get changes
                var changeResult = ObjectComparer.GetChanges(dto, existingUnit);

                if (!changeResult.HasChanges)
                {
                    return ApiResponse<READUnitDto>
                        .FailResponse("No changes detected");
                }
                // update entity
                ObjectComparer.ApplyChanges(dto, existingUnit);
                await _repo.UpdateAsync(existingUnit);

                // log audit
                var (oldValues, newValues) = ObjectComparer.SplitChanges(changeResult);
                await AddAudit(existingUnit.Id, "PATCH", oldValues, newValues);

                return ApiResponse<READUnitDto>
                    .SuccessResponse(null, "Updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.Message}");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READUnitDto>("Items Not Found.");

                await _repo.DeleteAsync(entity);

                return ApiResponse<READUnitDto>.FailResponse("Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.Message}");
            }
        }



        public async Task<ApiResponse<List<READUnitDto>>> GetVacants()
        {
            try
            {
                var entity = await _repo.GetVacantsAsync();

                if (entity == null)
                    return ApiResponse<List<READUnitDto>>.FailResponse("Items Not Found.");

                var vacants = entity.Select(it => it.ToReadDto()).ToList();
                return new ApiResponse<List<READUnitDto>>(vacants, "Fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<READUnitDto>>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }



        private async Task<(bool, string)> UnitChecker(int unitId, int propertyId, PATCHUnitDto unit) 
        {
            if (unit is null) return (true, "empty");
            if (unit.PropertyId.HasValue)
            {
                var propertyExist = await _propertyrepo.ExistAsync(unit.PropertyId.Value);
                if (!propertyExist) return (false, "Property Entered Does Exist");

            }
            if (unit.UnitTypeId.HasValue)
            {
                var unitTypeIdExist = await _systemcodeitemrepo.ExistsByIdAndCodeAsync(unit.UnitTypeId.Value, SystemCodeNames.Code.UnitType);
                if (!unitTypeIdExist) return (false, "UnitType Entered Does Exist");
            }
            var unitExist = await _repo.ExistsInPropertyAsync(unitId, propertyId);
            if (!unitExist) return (false, "Unit Does Not Exist In Property Selected");
            return (true, "success");
        }

        private async Task<(bool, string)> UnitChecker(CREATEUnitDto unit)
        {
            if (unit is null) return (true, "empty");

            // check if property Exist
            var propertyExist = await _propertyrepo.ExistAsync(unit.PropertyId);
            if (!propertyExist) return (false, "Property Entered Does Exist");

            // check if Unit Type Exist
            var unitTypeIdExist = await _systemcodeitemrepo.ExistsByIdAndCodeAsync(unit.UnitTypeId, SystemCodeNames.Code.UnitType);
            if (!unitTypeIdExist) return (false, "UnitType Entered Does Exist");

            return (true, "success");
        }

        private async Task AddAudit(int entityId, string action, object? oldValues, object? newValues)
        {
            var audit = new AuditTrail
            {
                AccountId = _currentuser.AccountId,
                UserId = _currentuser.UserId,
                EntityName = nameof(Unit),
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
