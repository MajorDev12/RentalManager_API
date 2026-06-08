using RentalManager.DTOs.Unit;
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
                Amount = dto.Amount,
                PropertyId = dto.PropertyId,
                UnitTypeId = dto.UnitTypeId,
                RentalTypeId = dto.RentalTypeId,
                BillingCycleId = dto.BillingCycleId,
                Floor = dto.Floor,
                StatusId = statusId,
                Notes = dto.Notes,
            };
        }

        public static Unit ToEntity(this UPDATEUnitDto dto)
        {
            return new Unit
            {
                Name = dto.Name,
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
                RentalTypeId = dto.RentalType.Id,
                RentalType = dto.RentalType.Item,
                BillingCycleId = dto.BillingCycle.Id,
                BillingCycle = dto.BillingCycle.Item,
                Floor = dto.Floor,
                UnitType = dto.UnitType.Item
            };
        }



        public static Unit UpdateEntity(this Unit updatedData, Unit existingData)
        {
            existingData.Name = updatedData.Name;
            existingData.Amount = updatedData.Amount;
            existingData.Notes = updatedData.Notes;
            existingData.PropertyId = updatedData.PropertyId;
            existingData.UnitTypeId = updatedData.UnitTypeId;

            return existingData;
        }


    }
}
