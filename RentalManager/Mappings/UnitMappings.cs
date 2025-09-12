using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class UnitMappings
    {
        public static Unit ToEntity(this CREATEUnitDto dto, int statusId)
        {
            return new Unit
            {
                Name = dto.Name,
                StatusId = statusId,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId,
                UnitTypeId = dto.UnitTypeId
            };
        }

        public static Unit ToEntity(this UPDATEUnitDto dto, int id)
        {
            return new Unit
            {
                Id = id,
                Name = dto.Name,
                StatusId = dto.StatusId,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId,
                UnitTypeId = dto.UnitTypeId
            };
        }


        public static READUnitDto ToReadDto(this Unit dto)
        {
            return new READUnitDto
            {
                Id = dto.Id,
                Name = dto.Name,
                StatusId = dto.StatusId,
                Status = dto.Status.Item,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyName = dto.Property.Name,
                PropertyId = dto.Property.Id,
                UnitTypeId = dto.UnitType.Id,
                UnitType = dto.UnitType.Name
            };
        }


        public static UPDATEUnitDto ToUpdateDto(this Unit dto)
        {
            return new UPDATEUnitDto
            {
                Name = dto.Name,
                StatusId = dto.StatusId,
                Amount = dto.Amount,
                Notes = dto.Notes,
                PropertyId = dto.PropertyId,
                UnitTypeId = dto.UnitTypeId,
            };
        }

        public static Unit UpdateEntity(this Unit updatedData, Unit existingData)
        {
            existingData.Name = updatedData.Name;
            existingData.StatusId = updatedData.StatusId;
            existingData.Amount = updatedData.Amount;
            existingData.Notes = updatedData.Notes;
            existingData.PropertyId = updatedData.PropertyId;
            existingData.UnitTypeId = updatedData.UnitTypeId;

            return existingData;
        }


        public static Unit UpdateStatusEntity(this Unit existingData, int statusId)
        {
            existingData.StatusId = statusId;

            return existingData;
        }

    }
}
