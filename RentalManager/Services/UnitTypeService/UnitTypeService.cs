using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.UnitTypeRepository;

namespace RentalManager.Services.UnitTypeService
{
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _repo;
        private readonly IPropertyRepository _propertyrepo;

        public UnitTypeService(IUnitTypeRepository repo, IPropertyRepository propertyrepo)
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
        }

        public async Task<ApiResponse<List<READUnitTypeDto>>> GetAll()
        {
            try
            {
                var types = await _repo.GetAllAsync();

                if (types == null)
                {
                    return new ApiResponse<List<READUnitTypeDto>>(null, "Data Not Found.");
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
                var codes = await _repo.GetByIdAsync(id);

                if (codes == null)
                {
                    return new ApiResponse<READUnitTypeDto>(null, "Data Not Found.");
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
                var types = await _repo.GetByPropertyIdAsync(id);

                if (types == null || types.Count == 0)
                {
                    return new ApiResponse<List<READUnitTypeDto>>(null, "Data Not Found.");
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
                var types = await _repo.AddAsync(entity);

                if (types == null)
                {
                    return new ApiResponse<READUnitTypeDto>(null, "Data Not Found.");
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

                var existing = await _repo.FindAsync(id);

                if (existing == null) return new ApiResponse<READUnitTypeDto>(null, "No Such Data.");

                var property = await _propertyrepo.FindAsync(type.PropertyId);

                if (property == null) return new ApiResponse<READUnitTypeDto>(null, "Property Does Not Exist.");


                var entity = type.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READUnitTypeDto>(null, "Data Not Found.");

                return new ApiResponse<READUnitTypeDto>(null, "Updated successfully.");
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
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READUnitTypeDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUnitTypeDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUnitTypeDto>($"Error Occurred: {ex.Message}");
            }
        }

    }
}
