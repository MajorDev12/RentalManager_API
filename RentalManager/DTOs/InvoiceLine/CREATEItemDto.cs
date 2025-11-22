namespace RentalManager.DTOs.InvoiceLine
{
    public class CREATEItemDto
    {
        public string TransactionCategory { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
