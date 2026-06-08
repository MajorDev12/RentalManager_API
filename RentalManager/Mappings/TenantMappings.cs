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
            TenantStatusId = dto.Status,
        };


        public static Tenant ToEntity(this CREATETenantDto dto, READUserDto savedUser, int statusId) => new Tenant
        {
            UserId = savedUser.Id,
            TenantStatusId = statusId,
        };


        public static Tenant ToEntity(this ASSIGNUnitDto dto) => new Tenant
        {
            Id = dto.tenantId,
            UnitId = dto.unitId,
            TenantStatusId = dto.statusId
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
            PropertyId = dto.User.PropertyId,
        };


        public static READTenantDto ToReadDto(this Tenant dto) => new READTenantDto
        {
            Id = dto.Id,
            FullName = $"{dto.User.FirstName}  {dto.User.LastName}",
            EmailAddress = dto.User.EmailAddress,
            MobileNumber = dto.User.MobileNumber,
            //User = dto.User.ToReadDto(),
            unitId = dto.Unit?.Id,
            Unit = dto.Unit?.Name,
            TenantStatusId = dto.TenantStatusId,
            TenantStatus = dto.TenantStatus.Item,
        };

        public static Tenant UpdateEntity(this Tenant updated, Tenant existing)
        {
            existing.UnitId = updated.UnitId;
            existing.TenantStatusId = updated.TenantStatusId;

            return existing;
        }

    }
}
