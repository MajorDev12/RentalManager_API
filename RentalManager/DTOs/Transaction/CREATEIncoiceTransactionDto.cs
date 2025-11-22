using RentalManager.DTOs.InvoiceLine;

namespace RentalManager.DTOs.Transaction
{
    public class CREATEIncoiceTransactionDto
    {
        public int UserId { get; set; }

        public int MonthFor { get; set; }

        public int YearFor { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public string Notes { get; set; } = string.Empty;

        public bool Combine { get; set; } = true;

        public List<CREATEItemDto> Item { get; set; } = null!;

    }
}
