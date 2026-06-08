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
                SystemCodeItemId = dto.NameId,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId
            };
        }

        public static UnitType ToEntity(this UPDATEUnitTypeDto dto)
        {
            return new UnitType
            {
                SystemCodeItemId = dto.NameId,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId
            };
        }


        public static READUnitTypeDto ToReadDto(this UnitType dto)
        {
            return new READUnitTypeDto
            {
                Id = dto.Id,
                NameId = dto.SystemCodeItemId,
                Name = dto.SystemCodeItem.Item,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId,
                PropertyName = dto.Property.Name
            };
        }





        public static UnitType UpdateEntity(this UnitType updatedData, UnitType existingData)
        {
            existingData.PropertyId = updatedData.PropertyId;
            existingData.Id = updatedData.Id;
            existingData.Notes = updatedData.Notes;

            return existingData;
        }
    }
}
