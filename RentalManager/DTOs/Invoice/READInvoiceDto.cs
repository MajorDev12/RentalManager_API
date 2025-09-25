using RentalManager.DTOs.Transaction;
using RentalManager.Models;

namespace RentalManager.DTOs.Invoice
{
    public class READInvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; } = string.Empty;

        public int TransactionId { get; set; }

        public READTransactionDto Transactions { get; set; } = null!;
    }
}
