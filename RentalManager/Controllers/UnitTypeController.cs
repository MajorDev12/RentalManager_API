using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitTypeController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetUnitType()
        {
            var types = await _context.UnitTypes.ToListAsync();

            if (!types.Any())
            {
                return NotFound(new ApiResponse<object>("There are no Utility Bills available."));
            }

            var typeDtos = types.Select(it => it.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READUnitTypeDto>>(typeDtos, ""));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitTypeById(int id)
        {
            var types = await _context.UnitTypes
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (types == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READUnitTypeDto>(types.ToReadDto(), ""));
        }




        [HttpPost]
        public async Task<IActionResult> AddUnitType([FromBody] CREATEUnitTypeDto AddedUnitType)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var type = AddedUnitType.ToEntity();

            _context.UnitTypes.Add(type);
            await _context.SaveChangesAsync();

            var createdUnitType = await _context.UnitTypes
                .FirstOrDefaultAsync(u => u.Id == type.Id);

            var unitTypeDto = createdUnitType?.ToReadDto();
            return Ok(new ApiResponse<READUnitTypeDto>(unitTypeDto!, "Unit added successfully."));
        }



        [HttpPut]
        public async Task<IActionResult> EditUnitType(int id, [FromBody] UPDATEUnitTypeDto dto)
        {

            var types = await _context.UnitTypes
                .FirstOrDefaultAsync(u => u.Id == id);

            if (types == null)
                return NotFound(new ApiResponse<object>("House not found."));


            // Manual update
            types.Name = dto.Name;
            types.Amount = dto.Amount;
            types.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<READUnitTypeDto>(types.ToReadDto(), ""));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitType(int id)
        {
            var types = await _context.UnitTypes.FindAsync(id);

            if (types == null)
            {
                return NotFound($"House with ID {id} was not found.");
            }

            _context.UnitTypes.Remove(types);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(null, "Data Deleted Successfully"));
        }



    }
}
