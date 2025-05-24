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
                Notes = dto.Notes
            };
        }


        public static READUnitTypeDto ToReadDto(this UnitType dto)
        {
            return new READUnitTypeDto
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                Notes = dto.Notes
            };
        }


        public static UPDATEUnitTypeDto ToUpdateDto(this UnitType dto)
        {
            return new UPDATEUnitTypeDto
            {
                Name = dto.Name,
                Amount = dto.Amount,
                Notes = dto.Notes,
            };
        }
    }
}
