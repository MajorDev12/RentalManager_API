namespace RentalManager.DTOs.Property
{
    public class CREATEPropertyUtilityDto
    {
        public int UtilityId { get; set; }
        public int BillingCycleId { get; set; }
        public decimal Amount { get; set; }
        public bool IsMetered { get; set; }
    }
}
