using RentalManager.DTOs.Role;
using RentalManager.DTOs.UnitType;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class RoleMappings
    {
        public static Role ToEntity(this CREATERoleDto dto)
        {
            return new Role
            {
                Name = dto.Name,
                IsEnabled = dto.IsEnabled,
                PropertyId = dto.PropertyId
            };
        }


        public static READRoleDto ToReadDto(this Role dto)
        {
            return new READRoleDto
            {
                Name = dto.Name,
                IsEnabled = dto.IsEnabled,
                PropertyName = dto.Property.Name
            };
        }


        public static UPDATERoleDto ToUpdateDto(this Role dto)
        {
            return new UPDATERoleDto
            {
                Name = dto.Name,
                IsEnabled = dto.IsEnabled,
                PropertyId = dto.PropertyId
            };
        }
    }
}
