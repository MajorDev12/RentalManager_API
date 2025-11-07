namespace RentalManager.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int? PropertyId { get; set; }

        public int? UnitId { get; set; }

        public int? TenantId { get; set; }

        public decimal? TotalIncome { get; set; }

        public decimal? TotalExpense { get; set; }

        public decimal? NetProfit => TotalIncome - TotalExpense;

        public int? Month { get; set; }  

        public int? Year { get; set; }


        public Property? Property { get; set; } 

        public Unit? Unit { get; set; }

        public Tenant? Tenant { get; set; }
    }
}
