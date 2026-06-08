using RentalManager.DTOs.Commons;

namespace RentalManager.DTOs.UtilityBill
{
    public class UtilityBillQueryFilter : QueryFilter
    {
        public string? PropertyName { get; set; }
        public string? UtilityName { get; set; }
        public string? BillingCycleName { get; set; }
        public bool? IsMetered { get; set; }
    }
}
