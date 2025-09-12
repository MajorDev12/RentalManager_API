using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UnitTypeRepository;
using System.Data;

namespace RentalManager.Services.UnitService
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IUnitTypeRepository _unittyperepo;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;

        public UnitService(
            IUnitRepository repo, 
            IPropertyRepository propertyrepo, 
            IUnitTypeRepository unittyperepo,
            ISystemCodeItemRepository systemcodeitemrepo
            )
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _unittyperepo = unittyperepo;
            _systemcodeitemrepo = systemcodeitemrepo;
        }

        public async Task<ApiResponse<List<READUnitDto>>> GetAll()
        {
            try
            {
                var units = await _repo.GetAllAsync();

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
                var unit = await _repo.GetByIdAsync(id);

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
                var units = await _repo.GetByPropertyIdAsync(id);

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
                var property = await _propertyrepo.FindAsync(unit.PropertyId);
                var unitType = await _unittyperepo.FindAsync(unit.UnitTypeId);
                var unitStatus = await _systemcodeitemrepo.GetByItemAsync("vacant");

                if (property == null || unitType == null) return new ApiResponse<READUnitDto>(null, "One Of The Items Provided Does Not Exist.");

                if (unitStatus == null) return new ApiResponse<READUnitDto>(null, "unit status does not exist");


                var entity = unit.ToEntity(unitStatus.Id);
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

                var existing = await _repo.FindAsync(id);

                if (existing == null) return new ApiResponse<READUnitDto>(null, "No Such Data.");

                var property = await _propertyrepo.FindAsync(unit.PropertyId);
                var unitType = await _unittyperepo.FindAsync(unit.UnitTypeId);
                var unitStatus = await _unittyperepo.FindAsync(unit.StatusId);

                if (property == null || unitType == null || unitStatus == null) return new ApiResponse<READUnitDto>(null, "One Of The Items Provided Does Not Exist.");

                var entity = unit.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READUnitDto>(null, "Data Not Found.");

                return new ApiResponse<READUnitDto>(null, "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitDto>("Error Occurred.");
            }
        }


        public async Task<ApiResponse<READUnitDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

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


    }
}
