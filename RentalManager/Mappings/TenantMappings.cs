using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.User;
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


        public static Tenant ToEntity(this CREATETenantDto dto, READUserDto savedUser, int statusId) => new Tenant
        {
            UserId = savedUser.Id,
            FullName = $"{savedUser.FirstName}  {savedUser.LastName}",
            EmailAddress = savedUser.EmailAddress,
            MobileNumber = savedUser.MobileNumber,
            Status = statusId,
        };


        public static Tenant ToEntity(this UPDATETenantDto dto, User savedUser) => new Tenant
        {
            FullName = $"{savedUser.FirstName}  {savedUser.LastName}",
            EmailAddress = savedUser.EmailAddress,
            MobileNumber = savedUser.MobileNumber,
        };


        public static Tenant ToEntity(this ASSIGNUnitDto dto) => new Tenant
        {
            Id = dto.tenantId,
            UnitId = dto.unitId,
            Status = dto.status
        };

        public static CREATEUserDto ToUserDto(this CREATETenantDto dto) => new CREATEUserDto
        {
            FirstName = dto.User.FirstName,
            LastName = dto.User.FirstName,
            EmailAddress = dto.User.EmailAddress,
            MobileNumber = dto.User.MobileNumber,
            AlternativeNumber = dto.User.AlternativeNumber,
            Password = "Tenant123",
            NationalId = dto.User.NationalId,
            IsActive = dto.User.IsActive,
            GenderId = dto.User.GenderId,
            RoleId = dto.User.RoleId,
            UserStatusId = dto.User.UserStatusId,
            PropertyId = dto.User.PropertyId
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
            tenant.FullName = $"{user.FirstName} {user.LastName}";
            tenant.EmailAddress = user.EmailAddress;
            tenant.MobileNumber = user.MobileNumber;

            return tenant;
        }

        public static Tenant UpdateEntity(this Tenant updated, Tenant existing)
        {
            existing.UnitId = updated.UnitId;
            existing.Status = updated.Status;

            return existing;
        }

    }
}
