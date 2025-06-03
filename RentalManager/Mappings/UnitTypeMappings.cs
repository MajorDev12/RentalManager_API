using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class UnitTypeMappings
    {
        public static UnitType ToEntity(this CREATEUnitTypeDto dto)
        {
            return new UnitType
            {
                Name = dto.Name,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId
            };
        }


        public static READUnitTypeDto ToReadDto(this UnitType dto)
        {
            return new READUnitTypeDto
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId,
                PropertyName = dto.Property.Name
            };
        }


        public static UPDATEUnitTypeDto ToUpdateDto(this UnitType dto)
        {
            return new UPDATEUnitTypeDto
            {
                Name = dto.Name,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId
            };
        }


        public static UnitType UpdateEntity(this UPDATEUnitTypeDto updatedData, UnitType existingData)
        {
            existingData.PropertyId = updatedData.PropertyId;
            existingData.Name = updatedData.Name;
            existingData.Amount = updatedData.Amount;
            existingData.Notes = updatedData.Notes;

            return existingData;
        }
    }
}
