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
                UtilityId = dto.UtilityId,
                PropertyId = dto.PropertyId,
                BillingCycleId = dto.BillingCycleId,
                UnitId = dto.UnitId,
                IsMetered = dto.IsMetered,
                Amount = dto.Amount,
            };
        }




        public static READUtilityBillDto ToReadDto(this UtilityBill bill)
        {
            return new READUtilityBillDto
            {
                Id = bill.Id,
                UtilityId = bill.Utility.Id,
                Name = bill.Utility.Item,
                Amount = bill.Amount,
                IsMetered = bill.IsMetered,
                PropertyId = bill.PropertyId,
                PropertyName = bill.Property.Name,
                BillingCycleId = bill.BillingCycle.Id,
                BillingCycleName = bill.BillingCycle.Item,
                UnitId = bill.UnitId,
                UnitName = bill.Unit?.Name,
            };
        }





    }
}
