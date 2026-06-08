using RentalManager.Services.AccountAccessService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class Property : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        public string Area { get; set; } = string.Empty;

        public string PhysicalAddress { get; set; } = string.Empty;

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int Floor { get; set; }

        public string? Notes { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;


        public int PropertyTypeId { get; set; }
        public SystemCodeItem PropertyType { get; set; } = null!;  

        public ICollection<Unit> Units { get; set; } = new List<Unit>();
        public ICollection<UtilityBill> Utilities { get; set; } = new List<UtilityBill>();
    }
}
