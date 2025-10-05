namespace RentalManager.DTOs.Transaction
{
    public class BalanceFilter
    {
        public int UtilityBillId { get; set; }
        public int? MonthFor { get; set; }
        public int? YearFor { get; set; }
        public int? UserId { get; set; }
    }
}
