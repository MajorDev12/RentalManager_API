namespace RentalManager.DTOs.Invoice
{
    public class CREATEInvoiceDto
    {
        public int Id { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public string Status { get; set; } = string.Empty;

        public int TransactionId { get; set; }
    }
}
