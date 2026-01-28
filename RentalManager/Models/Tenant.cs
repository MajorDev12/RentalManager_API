using RentalManager.Services.AccountAccessService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class Tenant : IAccountContext
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;


        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int AccountId { get; set; }

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int Status { get; set; }
        public SystemCodeItem TenantStatus { get; set; } = null!;
    }

}
