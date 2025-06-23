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

        public static Property ToUpdateEntity(this UPDATEPropertyDto dto) => new Property
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

        public static Property ToUpdateEntity(this Property updatedProperty, Property existing)
        {
            existing.Name = updatedProperty.Name;
            existing.Country = updatedProperty.Country;
            existing.County = updatedProperty.County;
            existing.Area = updatedProperty.Area;
            existing.PhysicalAddress = updatedProperty.PhysicalAddress;
            existing.Longitude = updatedProperty.Longitude;
            existing.Latitude = updatedProperty.Latitude;
            existing.Floor = updatedProperty.Floor;
            existing.EmailAddress = updatedProperty.EmailAddress;
            existing.MobileNumber = updatedProperty.MobileNumber;
            existing.Notes = updatedProperty.Notes;

            return existing;
        }




    }
}
