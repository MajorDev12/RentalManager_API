namespace RentalManager.DTOs.Transaction
{
    public class CREATEIncoiceTransactionDto
    {
        public int UserId { get; set; }

        public int PropertyId { get; set; }

        public int UtilityBillId { get; set; }

        public int MonthFor { get; set; }

        public int YearFor { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
