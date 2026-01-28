using Microsoft.AspNetCore.Identity;
using RentalManager.DTOs.Property;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepo;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ICurrentUser _currentuser;


        public PropertyService(
            IPropertyRepository propertyRepo,
            UserManager<ApplicationUser> usermanager,
            ICurrentUser currentuser
            )
        {
            _propertyRepo = propertyRepo;
            _usermanager = usermanager;
            _currentuser = currentuser;
        }



        public async Task<ApiResponse<List<READPropertyDto>>> GetAll()
        {
            try
            {
                var properties = await _propertyRepo.GetAllAsync(_currentuser);

                if(properties == null || !properties.Any()) return new ApiResponse<List<READPropertyDto>>(null, "Data Not Found");

                var propertyDtos = properties.Select(p => p.ToReadDto()).ToList();
                return new ApiResponse<List<READPropertyDto>>(propertyDtos, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READPropertyDto>>(null, $"error occured: {ex.InnerException?.Message ?? ex.Message}", false);
            }
        }

        public async Task<ApiResponse<READPropertyDto>> GetById(int id)
        {
            try
            {
                var property = await _propertyRepo.GetByIdAsync(_currentuser, id);

                if (property == null) return new ApiResponse<READPropertyDto>("Data Not Found");

                var propertyDto = property.ToReadDto();

                return new ApiResponse<READPropertyDto>(propertyDto, "Property Fetched successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READPropertyDto>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto, int accountId)
        {
            try
            {

                var property = dto.ToEntity();
                property.AccountId = accountId;

                if(property == null || property.AccountId <= 0) return new ApiResponse<READPropertyDto>("An error occurred");

                var savedProperty = await _propertyRepo.AddProperty(property);
                var readDto = savedProperty.ToReadDto();

                return new ApiResponse<READPropertyDto>(readDto, "Property created successfully.");
            }
            catch (Exception ex)
            {
                // Optionally log the error here
                return new ApiResponse<READPropertyDto>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Update(int id, UPDATEPropertyDto dto)
        {
            try
            {
                var existingProperty = await _propertyRepo.GetByIdAsync(_currentuser, id);

                if (existingProperty == null)
                    return new ApiResponse<READPropertyDto>(null, "Property does not exist", false);

                // chane to property entity
                var updateEntity = dto.ToUpdateEntity();

                // make the changes
                var updated = updateEntity.ToUpdateEntity(existingProperty);

                await _propertyRepo.UpdateAsync(updated);

                return new ApiResponse<READPropertyDto>(updated.ToReadDto(), "Updated successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<READPropertyDto>($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Delete(int id)
        {
            try
            {
                var property = await _propertyRepo.FindAsync(_currentuser, id);

                if (property == null)
                {
                    return new ApiResponse<READPropertyDto>("property was not found");
                }

                return new ApiResponse<READPropertyDto>(null, "Deleted Successfully");
            }
            catch
            {
                return new ApiResponse<READPropertyDto>("Error Occurred");
            }
        }
    }
}
