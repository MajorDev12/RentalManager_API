using RentalManager.DTOs.SystemCode;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class SystemCodeMappings
    {
        public static SystemCode ToEntity(this CreateSystemCodeDto dto) => new SystemCode
        {
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static ReadSystemCodeDto ToReadDto(this SystemCode dto) => new ReadSystemCodeDto
        {
            Id = dto.Id,
            Code = dto.Code,
            Notes = dto.Notes,
        };


        public static UpdateSystemCodeDto ToUpdateDto(this SystemCode dto) => new UpdateSystemCodeDto
        {
            Code = dto.Code,
            Notes = dto.Notes,
        };


    }
}
