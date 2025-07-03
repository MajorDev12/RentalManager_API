using RentalManager.DTOs.Tenant;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class TenantMappings
    {
        public static Tenant ToEntity(this CREATETenantDto dto) => new Tenant
        {
            User = dto.User.ToEntity(),
            Status = dto.Status,
        };


        public static Tenant ToEntity(this CREATETenantDto dto, User savedUser, int statusId) => new Tenant
        {
            UserId = savedUser.Id,
            FullName = $"{savedUser.FirstName}  {savedUser.LastName}",
            EmailAddress = savedUser.EmailAddress,
            MobileNumber = savedUser.MobileNumber,
            Status = statusId,
        };


        public static Tenant ToEntity(this UPDATETenantDto dto, User savedUser, int statusId, int id) => new Tenant
        {
            Id = id,
            UserId = savedUser.Id,
            FullName = $"{savedUser.FirstName}  {savedUser.LastName}",
            EmailAddress = savedUser.EmailAddress,
            MobileNumber = savedUser.MobileNumber,
            Status = statusId,
        };


        public static READTenantDto ToReadDto(this Tenant dto) => new READTenantDto
        {
            Id = dto.Id,
            FullName = dto.FullName,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            User = dto.User.ToReadDto(),
            unitId = dto.Unit?.Id,
            Unit = dto.Unit?.Name,
            TenantStatusId = dto.Status,
            TenantStatus = dto.TenantStatus.Item,
        };



        public static UPDATETenantDto ToUpdateDto(this Tenant dto) => new UPDATETenantDto
        {
            FullName = dto.FullName,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            User = dto.User.ToUpdateDto(),
        };


        public static Tenant UpdateEntity(this Tenant dto, Tenant tenant, User user)
        {
            tenant.UserId = user.Id;
            tenant.FullName = $"{user.FirstName} {user.LastName}";
            tenant.EmailAddress = user.EmailAddress;
            tenant.MobileNumber = user.MobileNumber;

            return tenant;
        }



    }
}
