using RentalManager.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Tenant
{
    public class CREATETenantDto
    {

        public CREATEUserDto User { get; set; } = null!;

        public int Status { get; set; }
    }
}
