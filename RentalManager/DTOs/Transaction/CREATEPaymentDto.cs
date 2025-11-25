namespace RentalManager.DTOs.Transaction
{
    public class CREATEPaymentDto
    {
        public int TenantId { get; set; }

        public decimal Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; } = string.Empty;
    }
}
