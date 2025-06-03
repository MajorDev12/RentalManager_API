using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var units = await _context.Units
                .Include(ub => ub.Property)
                .Include(ub => ub.UnitType)
                .ToListAsync();

            if (!units.Any())
            {
                return NotFound(new ApiResponse<object>(null, "There are no Units available."));
            }

            var unitDtos = units.Select(it => it.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READUnitDto>>(unitDtos, ""));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById(int id)
        {
            var unit = await _context.Units
                .Include(c => c.Property)
                .Include(ub => ub.UnitType)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (unit == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READUnitDto>(unit.ToReadDto(), ""));
        }


        [HttpPost]
        public async Task<IActionResult> AddUnit([FromBody] CREATEUnitDto AddedUnit)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var unit = AddedUnit.ToEntity();

            var unitType = await _context.UnitTypes.FindAsync(AddedUnit.UnitTypeId);
            var property = await _context.Properties.FindAsync(AddedUnit.PropertyId);

            if (unitType == null || property == null)
            {
                return BadRequest(new ApiResponse<object>("No such Property or Unit Type."));
            }

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            var createdUnit = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .FirstOrDefaultAsync(u => u.Id == unit.Id);

            var unitDto = createdUnit?.ToReadDto();
            return Ok(new ApiResponse<READUnitDto>(unitDto!, "Unit added successfully."));
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> EditUnit(int id, [FromBody] UPDATEUnitDto dto)
        {

            var unit = await _context.Units
                .Include(c => c.Property)
                .Include(c => c.UnitType)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
                return NotFound(new ApiResponse<object>("House not found."));


            // Manual update
            unit.Name = dto.Name;
            unit.Status = dto.Status;
            unit.Notes = dto.Notes;
            unit.PropertyId = dto.PropertyId;
            unit.UnitTypeId = dto.UnitTypeId;

            await _context.SaveChangesAsync();


            var updatedunit = await _context.Units
                .Include(c => c.Property)
                .Include(c => c.UnitType)
                .FirstOrDefaultAsync(u => u.Id == unit.Id);

            return Ok(new ApiResponse<READUnitDto>(updatedunit!.ToReadDto(), ""));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var unit = await _context.Units.FindAsync(id);

            if (unit == null)
            {
                return NotFound($"House with ID {id} was not found.");
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(null, "Data Deleted Successfully"));
        }





        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUnitsByProperty(int PropertyId)
        {
            var units = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .Where(u => u.PropertyId == PropertyId)
                .ToListAsync();

            if (units == null || !units.Any())
            {
                return NotFound(new ApiResponse<object>(null!, "There are no units for the specified property."));
            }

            var unitDtos = units.Select(u => u.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READUnitDto>>(unitDtos, "Units fetched successfully."));
        }




    }
}
