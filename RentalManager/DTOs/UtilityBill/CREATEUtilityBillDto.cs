using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.UtilityBill
{
    public class CREATEUtilityBillDto
    {
        public int UtilityId { get; set; }

        public int PropertyId { get; set; }

        public int BillingCycleId { get; set; }

        public int? UnitId { get; set; }

        public decimal Amount { get; set; }

        public bool IsMetered { get; set; } = false;
    }
}
