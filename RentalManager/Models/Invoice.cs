namespace RentalManager.Models
{
    public class Invoice : AuditableEntity
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; } = string.Empty; // Draft, Sent, Paid, PartiallyPaid, Overdue

        public bool Combine { get; set; } = true;

        public int TransactionId { get; set; }

        public Transaction Transactions { get; set; } = null!;

    }
}
