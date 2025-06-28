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
            SystemCodeId = dto.SystemCodeId
        };

        public static SystemCodeItem ToEntity(this UPDATESystemCodeItemDto dto, int id) => new SystemCodeItem
        {
            Id = id,
            Item = dto.Item,
            Notes = dto.Notes,
        };

        public static READSystemCodeItemDto ToReadDto(this SystemCodeItem dto) => new READSystemCodeItemDto
        {
            Id = dto.Id,
            Item = dto.Item,
            Notes = dto.Notes,
            SystemCodeId = dto.SystemCodeId,
            SystemCodeName = dto.SystemCode.Code,
        };

        public static UPDATESystemCodeItemDto ToUpdateDto(this SystemCodeItem dto) => new UPDATESystemCodeItemDto
        {
            Item = dto.Item,
            Notes = dto.Notes,
        };

        public static SystemCodeItem ToUpdateEntity(this SystemCodeItem UpdatedItem, SystemCodeItem existingItem)
        {
            existingItem.Item = UpdatedItem.Item;
            existingItem.Notes = UpdatedItem.Notes;
            existingItem.UpdatedOn = UpdatedItem.UpdatedOn;

            return existingItem;
        }

    }
}
