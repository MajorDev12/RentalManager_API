namespace RentalManager.DTOs.Transaction
{
    public class TenantBalanceDto
    {
        public int UserId { get; set; }
        public int Month { get; set; }   // 1 = Jan, 2 = Feb, etc.
        public int Year { get; set; }
        public decimal TotalCharges { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal Balance { get; set; }
    }
}
