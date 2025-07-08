using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.SystemCodeRepository;

namespace RentalManager.Services.SystemCodeItemService
{
    public class SystemCodeItemService : ISystemCodeItemService
    {
        private readonly ISystemCodeItemRepository _repo;
        public SystemCodeItemService(ISystemCodeItemRepository repo)
        {
            _repo = repo;
        }


        public async Task<ApiResponse<List<READSystemCodeItemDto>>> GetAll()
        {
            try
            {
                var items = await _repo.GetAllAsync();

                if (items == null)
                {
                    return new ApiResponse<List<READSystemCodeItemDto>>(null, "Data Not Found.");
                }

                var codeDtos = items.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READSystemCodeItemDto>>(codeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READSystemCodeItemDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READSystemCodeItemDto>> GetById(int id)
        {
            try
            {
                var item = await _repo.GetByIdAsync(id);

                if (item == null)
                {
                    return new ApiResponse<READSystemCodeItemDto>(null, "Data Not Found.");
                }

                var itemDto = item.ToReadDto();

                return new ApiResponse<READSystemCodeItemDto>(itemDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeItemDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READSystemCodeItemDto>> Add(CREATESystemCodeItemDto item)
        {
            try
            {

                var existingCode = await _repo.FindAsync(item.SystemCodeId);

                if (existingCode == null) return new ApiResponse<READSystemCodeItemDto>(null, "No Such Data.");


                var entity = item.ToEntity();
                var items = await _repo.AddAsync(entity);

                if (items == null)
                {
                    return new ApiResponse<READSystemCodeItemDto>(null, "Data Not Found.");
                }

                return new ApiResponse<READSystemCodeItemDto>(null, "SystemCodeItem Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeItemDto>($"Error Occurred: {ex.InnerException.Message}");
            }
        }

        public async Task<ApiResponse<READSystemCodeItemDto>> Update(int id, UPDATESystemCodeItemDto item)
        {
            try
            {

                var existing = await _repo.FindAsync(id);

                if (existing == null) return new ApiResponse<READSystemCodeItemDto>(null, "No Such Data.");


                var entity = item.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READSystemCodeItemDto>(null, "Data Not Found.");

                return new ApiResponse<READSystemCodeItemDto>(null, "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeItemDto>("Error Occurred.");
            }
        }


        public async Task<ApiResponse<READSystemCodeItemDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READSystemCodeItemDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READSystemCodeItemDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeItemDto>($"Error Occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<READSystemCodeItemDto>>> GetByCode(string codeName)
        {
            try
            {
                var items = await _repo.GetByCodeAsync(codeName);

                if (items == null || items.Count == 0)
                {
                    return new ApiResponse<List<READSystemCodeItemDto>>(null, "Data Not Found.");
                }

                var itemsDto = items.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READSystemCodeItemDto>>(itemsDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READSystemCodeItemDto>>("Error Occurred");
            }
        }
    }
}
