using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
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

        public UnitService(
            IUnitRepository repo, 
            IPropertyRepository propertyrepo, 
            IUnitTypeRepository unittyperepo,
            ISystemCodeItemRepository systemcodeitemrepo,
            ICurrentUser currentuser
            )
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _unittyperepo = unittyperepo;
            _systemcodeitemrepo = systemcodeitemrepo;
            _currentuser = currentuser;
        }

        public async Task<ApiResponse<List<READUnitDto>>> GetAll()
        {
            try
            {
                var units = await _repo.GetAllAsync(_currentuser);

                if (units == null)
                {
                    return new ApiResponse<List<READUnitDto>>(null, "Data Not Found.");
                }

                var unitDtos = units.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUnitDto>>(unitDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUnitDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUnitDto>> GetById(int id)
        {
            try
            {
                var unit = await _repo.GetByIdAsync(_currentuser, id);

                if (unit == null)
                {
                    return new ApiResponse<READUnitDto>(null, "Data Not Found.");
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
                var units = await _repo.GetByPropertyIdAsync(_currentuser, id);

                if (units == null || units.Count == 0)
                {
                    return new ApiResponse<List<READUnitDto>>(null, "Data Not Found.");
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
                var property = await _propertyrepo.FindAsync(_currentuser, unit.PropertyId);
                var unitType = await _unittyperepo.FindAsync(_currentuser, unit.UnitTypeId);
                var unitStatus = await _systemcodeitemrepo.GetByItemAsync("Vacant");

                if (property == null || unitType == null) return new ApiResponse<READUnitDto>(null, "One Of The Items Provided Does Not Exist.");

                if (unitStatus == null) return new ApiResponse<READUnitDto>(null, "unit status does not exist");


                var entity = unit.ToEntity(unitStatus.Id);
                entity.AccountId = _currentuser.AccountId;

                var types = await _repo.AddAsync(entity);

                if (types == null)
                {
                    return new ApiResponse<READUnitDto>(null, "Data Not Found.");
                }


                return new ApiResponse<READUnitDto>(null, "Unit Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.InnerException?.Message} ");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Update(int id, UPDATEUnitDto unit)
        {
            try
            {

                var existing = await _repo.FindAsync(_currentuser, id);

                if (existing == null) return new ApiResponse<READUnitDto>(null, "No Such Data.", false);

                var entity = unit.ToEntity();

                if (!HasChanged(existing, entity))
                {
                    return new ApiResponse<READUnitDto>(null, "No changes detected.", false);
                }


                var property = await _propertyrepo.FindAsync(_currentuser, unit.PropertyId);
                var unitType = await _unittyperepo.FindAsync(_currentuser, unit.UnitTypeId);

                if (property == null || unitType == null) 
                    return new ApiResponse<READUnitDto>(null, "One Of The Items Provided Does Not Exist.");


                var updatedEntity = entity.UpdateEntity(existing);
                await _repo.UpdateAsync(entity);


                return new ApiResponse<READUnitDto>(updatedEntity.ToReadDto(), "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<ApiResponse<READUnitDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(_currentuser, id);

                if (entity == null)
                    return new ApiResponse<READUnitDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUnitDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>($"Error Occurred: {ex.Message}");
            }
        }


        private static bool HasChanged(Unit existing, Unit updated)
        {
            return
                existing.Name != updated.Name ||
                existing.Amount != updated.Amount ||
                existing.UnitTypeId != updated.UnitTypeId ||
                existing.PropertyId != updated.PropertyId;
        }

    }
}
