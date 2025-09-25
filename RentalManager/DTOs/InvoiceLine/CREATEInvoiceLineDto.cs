namespace RentalManager.DTOs.InvoiceLine
{
    public class CREATEInvoiceLineDto
    {
        public int InvoiceId { get; set; }

        public string TransactionCategory { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
