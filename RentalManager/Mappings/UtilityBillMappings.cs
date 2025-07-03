using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class UtilityBillMappings
    {
        public static UtilityBill ToEntity(this CREATEUtilityBillDto dto)
        {
            return new UtilityBill
            {
                Name = dto.Name,
                Amount = dto.Amount,
                PropertyId = dto.PropertyId
            };
        }

        public static UtilityBill ToEntity(this UPDATEUtilityBillDto dto, int id)
        {
            return new UtilityBill
            {
                Id = id,
                Name = dto.Name,
                Amount = dto.Amount,
                PropertyId = dto.PropertyId
            };
        }


        public static READUtilityBillDto ToReadDto(this UtilityBill bill)
        {
            return new READUtilityBillDto
            {
                Id = bill.Id,
                Name = bill.Name,
                Amount = bill.Amount,
                PropertyId = bill.PropertyId,
                PropertyName = bill.Property.Name
            };
        }


        public static UPDATEUtilityBillDto ToUpdateDto(this UtilityBill bill)
        {
            return new UPDATEUtilityBillDto
            {
                Name = bill.Name,
                Amount = bill.Amount,
                PropertyId = bill.PropertyId
            };
        }

        public static UtilityBill UpdateEntity(this UtilityBill updatedData, UtilityBill existingData)
        {
            existingData.PropertyId = updatedData.PropertyId;
            existingData.Name = updatedData.Name;
            existingData.Amount = updatedData.Amount;

            return existingData;
        }

    }
}
