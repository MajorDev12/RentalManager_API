using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }


        public int? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }


        public int? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }

    }

}
