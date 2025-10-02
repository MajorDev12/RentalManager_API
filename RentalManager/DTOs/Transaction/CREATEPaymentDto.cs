namespace RentalManager.DTOs.Transaction
{
    public class CREATEPaymentDto
    {
        public int TenantId { get; set; }

        public int UtilityBillId { get; set; }

        public int TransactionTypeId { get; set; }

        public decimal Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int MonthFor { get; set; }

        public int YearFor { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
