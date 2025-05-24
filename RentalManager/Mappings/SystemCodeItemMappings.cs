using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class SystemCodeItemMappings
    {
        public static SystemCodeItem ToEntity(this CREATESystemCodeItemDto dto) => new SystemCodeItem
        {
            Item = dto.Item,
            Notes = dto.Notes,
        };

        public static READSystemCodeItemDto ToReadDto(this SystemCodeItem dto) => new READSystemCodeItemDto
        {
            Id = dto.Id,
            Item = dto.Item,
            Notes = dto.Notes,
        };

        public static UPDATESystemCodeItemDto ToUpdateDto(this SystemCodeItem dto) => new UPDATESystemCodeItemDto
        {
            Item = dto.Item,
            Notes = dto.Notes,
        };

    }
}
