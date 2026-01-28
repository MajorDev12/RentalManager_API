namespace RentalManager.DTOs.InvoiceLine
{
    public class CREATEItemDto
    {
        public int TransactionCategoryId { get; set; }

        public int? UtilityBillId { get; set; }

        public decimal Amount { get; set; }
    }
}
