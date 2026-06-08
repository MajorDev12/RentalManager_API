using RentalManager.Services.AccountAccessService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;


        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int TenantStatusId { get; set; }
        public SystemCodeItem TenantStatus { get; set; } = null!;
    }

}
