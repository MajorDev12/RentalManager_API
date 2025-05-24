using RentalManager.DTOs.Tenant;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class TenantMappings
    {
        public static Tenant ToEntity(this CREATETenantDto dto) => new Tenant
        {
            User = dto.User.ToEntity(),
            UnitId = dto.UnitId,
            Status = dto.Status,
        };

        public static Tenant ToEntity(this CREATETenantDto dto, User savedUser, int unitId, int statusId) => new Tenant
        {
            UserId = savedUser.Id,
            FullName = $"{savedUser.FirstName}  {savedUser.LastName}",
            EmailAddress = savedUser.EmailAddress,
            MobileNumber = savedUser.MobileNumber,
            UnitId = unitId,
            Status = statusId,
        };


        public static READTenantDto ToReadDto(this Tenant dto) => new READTenantDto
        {
            Id = dto.Id,
            FullName = dto.FullName,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            User = dto.User.ToReadDto(),
            Unit = dto.Unit?.Name,
            TenantStatus = dto.TenantStatus.Item,
        };



        public static UPDATETenantDto ToUpdateDto(this Tenant dto) => new UPDATETenantDto
        {
            FullName = dto.FullName,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            User = dto.User.ToUpdateDto(),
            UnitId = dto.UnitId,
            Status = dto.Status,
        };


        public static Tenant UpdateEntity(this UPDATETenantDto dto, Tenant tenant, User user, int unitId, int statusId)
        {
            tenant.UserId = user.Id;
            tenant.FullName = $"{user.FirstName} {user.LastName}";
            tenant.EmailAddress = user.EmailAddress;
            tenant.MobileNumber = user.MobileNumber;
            tenant.UnitId = unitId;
            tenant.Status = statusId;

            return tenant;
        }



    }
}
