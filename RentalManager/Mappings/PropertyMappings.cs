using RentalManager.DTOs.Property;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class PropertyMappings
    {
        public static Property ToEntity(this CREATEPropertyDto dto) => new Property
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
        };



        public static UPDATEPropertyDto ToUpdateDto(this Property dto) => new UPDATEPropertyDto
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
        };




    }
}
