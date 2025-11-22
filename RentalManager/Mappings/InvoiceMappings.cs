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
            Status = dto.Status,
            TransactionId = dto.TransactionId
        };



        public static READInvoiceDto ToReadDto(this Invoice dto) => new READInvoiceDto
        {
            InvoiceNumber = dto.InvoiceNumber,
            TotalAmount = dto.TotalAmount,
            Status = dto.Status,
            TransactionId = dto.TransactionId,
        };



        public static InvoiceLine ToEntityLineDto(this CREATEInvoiceLineDto dto, Invoice invoice) => new InvoiceLine
        {
            InvoiceId = invoice.Id,
            Amount = dto.Amount,
            TransactionCategory = dto.TransactionCategory
        };
    }
}
