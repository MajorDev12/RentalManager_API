using RentalManager.DTOs.SystemCode;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class SystemCodeMappings
    {
        public static SystemCode ToEntity(this CREATESystemCodeDto dto) => new SystemCode
        {
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static SystemCode ToEntity(this UPDATESystemCodeDto dto, int id) => new SystemCode
        {
            Id = id,
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static READSystemCodeDto ToReadDto(this SystemCode dto) => new READSystemCodeDto
        {
            Id = dto.Id,
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static UPDATESystemCodeDto ToUpdateDto(this SystemCode dto) => new UPDATESystemCodeDto
        {
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static SystemCode ToUpdateEntity(this SystemCode UpdatedSystemCode, SystemCode existingCode)
        {
            existingCode.Code = UpdatedSystemCode.Code;
            existingCode.Notes = UpdatedSystemCode.Notes;
            existingCode.UpdatedOn = UpdatedSystemCode.UpdatedOn;

            return existingCode;
        }



    }
}
