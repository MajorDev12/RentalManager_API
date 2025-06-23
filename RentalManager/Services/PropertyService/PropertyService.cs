using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;

namespace RentalManager.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepo;

        public PropertyService(IPropertyRepository propertyRepo)
        {
            _propertyRepo = propertyRepo;
        }



        public async Task<ApiResponse<List<READPropertyDto>>> GetAll()
        {
            try
            {
                var properties = await _propertyRepo.GetAllAsync();

                if(properties == null || !properties.Any()) return new ApiResponse<List<READPropertyDto>>(null, "Data Not Found");

                var propertyDtos = properties.Select(p => p.ToReadDto()).ToList();
                return new ApiResponse<List<READPropertyDto>>(propertyDtos, "Fetched Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READPropertyDto>>("error occured");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> GetById(int id)
        {
            try
            {
                var property = await _propertyRepo.GetByIdAsync(id);

                if (property == null) return new ApiResponse<READPropertyDto>("Data Not Found");

                var propertyDto = property.ToReadDto();

                return new ApiResponse<READPropertyDto>(propertyDto, "Property Fetched successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READPropertyDto>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto)
        {
            try
            {

                var property = dto.ToEntity();

                if(property == null) return new ApiResponse<READPropertyDto>("An error occurred");

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

        public async Task<ApiResponse<READPropertyDto>> Update(UPDATEPropertyDto dto)
        {
            try
            {
                var entity = dto.ToUpdateEntity();

                var updatedProperty = await _propertyRepo.UpdateAsync(entity);

                if (updatedProperty == null)
                    return new ApiResponse<READPropertyDto>(null, "Property Not Found");

                return new ApiResponse<READPropertyDto>(updatedProperty.ToReadDto(), "Updated successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<READPropertyDto>("Error Occurred");
            }
        }

        public async Task<ApiResponse<READPropertyDto>> Delete(int id)
        {
            try
            {
                var property = await _propertyRepo.FindAsync(id);

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
