using RentalManager.DTOs.Role;
using RentalManager.DTOs.SystemCode;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeRepository;

namespace RentalManager.Services.SystemCodeService
{
    public class SystemCodeService : ISystemCodeService
    {
        private readonly ISystemCodeRepository _repo;
        public SystemCodeService(ISystemCodeRepository repo)
        {
            _repo = repo;
        }
        public async Task<ApiResponse<List<READSystemCodeDto>>> GetAll()
        {
            try
            {
                var codes = await _repo.GetAllAsync();

                if (codes == null)
                {
                    return new ApiResponse<List<READSystemCodeDto>>(null, "Data Not Found.");
                }

                var codeDtos = codes.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READSystemCodeDto>>(codeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READSystemCodeDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READSystemCodeDto>> GetById(int id)
        {
            try
            {
                var codes = await _repo.GetByIdAsync(id);

                if (codes == null)
                {
                    return new ApiResponse<READSystemCodeDto>(null, "Data Not Found.");
                }

                var codeDtos = codes.ToReadDto();

                return new ApiResponse<READSystemCodeDto>(codeDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READSystemCodeDto>> Add(CREATESystemCodeDto code)
        {
            try
            {
                var entity = code.ToEntity();
                var codes = await _repo.AddAsync(entity);

                if (codes == null)
                {
                    return new ApiResponse<READSystemCodeDto>(null, "Data Not Found.");
                }

                var codeDtos = codes.ToReadDto();

                return new ApiResponse<READSystemCodeDto>(codeDtos, "SystemCode Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READSystemCodeDto>> Update(int id, UPDATESystemCodeDto code)
        {
            try
            {

                var existing = await _repo.FindAsync(id);

                if(existing == null) return new ApiResponse<READSystemCodeDto>(null, "No Such Data.");


                var entity = code.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READSystemCodeDto>(null, "Data Not Found.");

                return new ApiResponse<READSystemCodeDto>(updated.ToReadDto(), "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeDto>("Error Occurred.");
            }

        }


        public async Task<ApiResponse<READSystemCodeDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READSystemCodeDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READSystemCodeDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READSystemCodeDto>($"Error Occurred: {ex.Message}");
            }

        }


    }
}
