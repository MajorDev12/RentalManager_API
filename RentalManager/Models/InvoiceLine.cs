namespace RentalManager.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoices { get; set; } = null!;

        public string TransactionCategory { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public ICollection<InvoiceLine> InvoiceLines { get; set; } = null!;
        public ICollection<Transaction> Payments { get; set; } = null!;
    }
}
