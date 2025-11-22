namespace RentalManager.DTOs.Transaction
{
    public class InvoiceMappingContext
    {
        public int PropertyId { get; set; }
        public int UnitId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? UtilityBillId { get; set; }
        public int TransactionCategoryId { get; set; }
    }

}
