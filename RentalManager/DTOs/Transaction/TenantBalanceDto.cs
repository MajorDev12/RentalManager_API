using RentalManager.Models;

namespace RentalManager.DTOs.Transaction
{
    public class TenantBalanceDto
    {
        public int UserId { get; set; }

        public string UnitName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string PropertyName { get; set; } = string.Empty;

        public int Month { get; set; }  

        public int Year { get; set; }

        public decimal TotalCharges { get; set; }

        public decimal TotalPayments { get; set; }

        public decimal Balance { get; set; }


    }
}
