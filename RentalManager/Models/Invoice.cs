namespace RentalManager.Models
{
    public class Invoice : AuditableEntity
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Balance => TotalAmount - AmountPaid;

        public string Status { get; set; } = string.Empty; // Draft, Sent, Paid, PartiallyPaid, Overdue

    }
}
