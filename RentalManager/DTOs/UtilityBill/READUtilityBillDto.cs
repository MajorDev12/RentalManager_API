using System.ComponentModel.DataAnnotations;
using RentalManager.Models;

namespace RentalManager.DTOs.UtilityBill
{
    public class READUtilityBillDto
    {
        public int Id { get; set; }

        public int UtilityId { get; set; }
        public string Name { get; set; } = string.Empty;


        public decimal Amount { get; set; }

        public bool IsMetered { get; set; }
        

        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;

        public int BillingCycleId { get; set; }
        public string BillingCycleName { get; set; } = string.Empty;

        public int? UnitId { get; set; }
        public string? UnitName { get; set; } = string.Empty;
    }
}
