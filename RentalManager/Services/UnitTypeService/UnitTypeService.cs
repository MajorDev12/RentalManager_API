using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.UnitTypeRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.UnitTypeService
{
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly ICurrentUser _currentuser;

        public UnitTypeService(
            IUnitTypeRepository repo,
            IPropertyRepository propertyrepo,
            ICurrentUser currentuser)
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _currentuser = currentuser;
        }

        public async Task<ApiResponse<List<READUnitTypeDto>>> GetAll()
        {
            try
            {
                var types = await _repo.GetAllAsync(_currentuser);

                if (types == null)
                {
                    return new ApiResponse<List<READUnitTypeDto>>(null, "Items Not Found.");
                }

                var typeDtos = types.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUnitTypeDto>>(typeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUnitTypeDto>>("Error Occurred");
            }
        }

        public async Task<ApiResponse<READUnitTypeDto>> GetById(int id)
        {
            try
            {
                var codes = await _repo.GetByIdAsync(_currentuser, id);

                if (codes == null)
                {
                    return new ApiResponse<READUnitTypeDto>(null, "Items Not Found.");
                }

                var codeDtos = codes.ToReadDto();

                return new ApiResponse<READUnitTypeDto>(codeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitTypeDto>("Error Occurred");
            }
        }

        public async Task<ApiResponse<List<READUnitTypeDto>>> GetByPropertyId(int id)
        {
            try
            {
                var types = await _repo.GetByPropertyIdAsync(_currentuser, id);

                if (types == null || types.Count == 0)
                {
                    return new ApiResponse<List<READUnitTypeDto>>(null, "Items Not Found.");
                }

                var typeDto = types.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUnitTypeDto>>(typeDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUnitTypeDto>>("Error Occurred");
            }
        }

        public async Task<ApiResponse<READUnitTypeDto>> Add(CREATEUnitTypeDto type)
        {
            try
            {
                var property = await _propertyrepo.FindAsync(type.PropertyId);

                if (property == null) return new ApiResponse<READUnitTypeDto>(null, "Property Does Not Exist.");


                var entity = type.ToEntity();
                entity.AccountId = _currentuser.AccountId;

                var types = await _repo.AddAsync(entity);

                if (types == null)
                {
                    return new ApiResponse<READUnitTypeDto>(null, "Items Not Found.");
                }


                return new ApiResponse<READUnitTypeDto>(null, "Unit Type Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitTypeDto>($"Error Occurred: {ex.InnerException.Message} ");
            }
        }


        public async Task<ApiResponse<READUnitTypeDto>> Update(int id, UPDATEUnitTypeDto type)
        {
            try
            {

                var existing = await _repo.FindAsync(_currentuser, id);

                if (existing == null) return new ApiResponse<READUnitTypeDto>(null, "No Such Items.");

                var property = await _propertyrepo.FindAsync(type.PropertyId);

                if (property == null) return new ApiResponse<READUnitTypeDto>(null, "Property Does Not Exist.");


                var entity = type.ToEntity();

                if (!hasChanged(existing, entity))
                {
                    return new ApiResponse<READUnitTypeDto>(null, "No changes detected.", false);
                }

                var updatedEntity = entity.UpdateEntity(existing);

                await _repo.UpdateAsync(updatedEntity);


                return new ApiResponse<READUnitTypeDto>(updatedEntity.ToReadDto(), "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitTypeDto>("Error Occurred.");
            }
        }


        public async Task<ApiResponse<READUnitTypeDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(_currentuser, id);

                if (entity == null)
                    return new ApiResponse<READUnitTypeDto>("Items Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUnitTypeDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitTypeDto>($"Error Occurred: {ex.Message}");
            }
        }


        private static bool hasChanged(UnitType existing, UnitType updated)
        {
            return
                existing.SystemCodeItem.Item != updated.SystemCodeItem.Item ||
                existing.Notes != updated.Notes ||
                existing.PropertyId != updated.PropertyId;
        }

    }
}
