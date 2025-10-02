using RentalManager.DTOs.Invoice;
using RentalManager.DTOs.InvoiceLine;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class InvoiceMappings
    {
        public static Invoice ToEntity(this CREATEInvoiceDto dto, string InvoiceNumber) => new Invoice
        {
            InvoiceNumber = InvoiceNumber,
            TotalAmount = dto.TotalAmount,
            AmountPaid = dto.AmountPaid,
            Balance = dto.TotalAmount - dto.AmountPaid,
            Status = dto.Status,
            TransactionId = dto.TransactionId
        };



        public static READInvoiceDto ToReadDto(this Invoice dto) => new READInvoiceDto
        {
            InvoiceNumber = dto.InvoiceNumber,
            TotalAmount = dto.TotalAmount,
            AmountPaid = dto.AmountPaid,
            Balance = dto.Balance,
            Status = dto.Status,
            TransactionId = dto.TransactionId,
            Transactions = dto.Transactions.ToReadDto(),
        };



        public static InvoiceLine ToEntityLineDto(this CREATEInvoiceLineDto dto, Invoice invoice) => new InvoiceLine
        {
            InvoiceId = invoice.Id,
            Amount = invoice.AmountPaid,
        };
    }
}
