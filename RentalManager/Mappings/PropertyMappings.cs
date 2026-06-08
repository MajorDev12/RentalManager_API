using RentalManager.DTOs.Property;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class PropertyMappings
    {
        public static Property ToEntity(this CREATEPropertyDto dto, int accountId) => new Property
        {
            Name = dto.Name,
            Country = dto.Country,
            County = dto.County,
            Area = dto.Area,
            PhysicalAddress = dto.PhysicalAddress,
            Longitude = dto.Longitude,
            Latitude = dto.Latitude,
            Floor = dto.Floor,
            Notes = dto.Notes,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            AccountId = accountId,
            PropertyTypeId = dto.PropertyTypeId
        };



        public static READPropertyDto ToReadDto(this Property dto) => new READPropertyDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Country = dto.Country,
            County = dto.County,
            Area = dto.Area,
            PhysicalAddress = dto.PhysicalAddress,
            Longitude = dto.Longitude,
            Latitude = dto.Latitude,
            Floor = dto.Floor,
            Notes = dto.Notes,
            EmailAddress = dto.EmailAddress,
            MobileNumber = dto.MobileNumber,
            PropertyTypeId = dto.PropertyTypeId,
            PropertyTypeName = dto.PropertyType.Item,
        };



    }
}
