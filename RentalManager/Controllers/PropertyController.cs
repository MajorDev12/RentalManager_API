using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.SystemCode;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PropertyController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Properties")]
        public async Task<ActionResult<IEnumerable<READPropertyDto>>> GetProperties()
        {
            var properties = await _context.Properties.ToListAsync();

            var propertyDtos = properties.Select(p => p.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READPropertyDto>>(propertyDtos, ""));
        }




        [HttpGet("Properties/{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(pr => pr.Id == id);

            if (property == null)
            {
                return NotFound(new ApiResponse<object>("property was not found"));
            }

            return Ok(new ApiResponse<READPropertyDto>(property.ToReadDto(), ""));
        }





        [HttpPost("Property/{id}")]
        public async Task<IActionResult> AddProperty([FromBody] CREATEPropertyDto AddedProperty)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var property = AddedProperty.ToEntity();

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            var propertyDto = property.ToReadDto();
            return Ok(new ApiResponse<READPropertyDto>(propertyDto, "Property added successfully."));
        }






        [HttpPut("Property/{id}")]
        public async Task<IActionResult> EditProperty(int id, [FromBody] UPDATEPropertyDto dto)
        {

            var property = await _context.Properties.FindAsync(id);

            if (property == null)
                return NotFound(new ApiResponse<object>("Property not found."));



            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed", errors));
            }


            // Manual update
            property.Name = dto.Name;
            property.Country = dto.Country;
            property.County = dto.County;
            property.Area = dto.Area;
            property.PhysicalAddress = dto.PhysicalAddress;
            property.Longitude = dto.Longitude;
            property.Latitude = dto.Latitude;
            property.Floor = dto.Floor;
            property.EmailAddress = dto.EmailAddress;
            property.MobileNumber = dto.MobileNumber;
            property.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<READPropertyDto>(property.ToReadDto(), "Property updated successfully."));

        }



        [HttpDelete("Property/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound(new ApiResponse<object>("property Id was not found"));
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            var propertyDtos = property.ToReadDto();

            return Ok(new ApiResponse<READPropertyDto>(propertyDtos, "Property deleted successfuly"));
        }






    }
}
