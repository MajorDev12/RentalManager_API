using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;


        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int Status { get; set; }
        public SystemCodeItem TenantStatus { get; set; } = null!;
    }

}
