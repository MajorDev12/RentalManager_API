using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.SystemCode;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SystemCodeController (ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadSystemCodeDto>>> GetSystemCodes()
        {
            var systemCodes = await _context.SystemCodes.ToListAsync();

            if (systemCodes == null)
            {
                return NotFound($"There is no SystemCode available.");
            }

            var systemCodeDtos = systemCodes.Select(sc => sc.ToReadDto()).ToList();

            return Ok(systemCodeDtos);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetSystemCodeById(int id)
        {
            var systemCode = await _context.SystemCodes.FirstOrDefaultAsync(e => e.Id == id);

            if (systemCode == null)
            {
                return NotFound();
            }

            return Ok(systemCode.ToReadDto());
        }


        [HttpPost]
        public async Task<IActionResult> AddSystemCode([FromBody] CreateSystemCodeDto AddedSystemCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var systemCode = AddedSystemCode.ToEntity();

            _context.SystemCodes.Add(systemCode);
            await _context.SaveChangesAsync();

            var allSystemCodes = await _context.SystemCodes.ToListAsync();

            var codeDtos = allSystemCodes.Select(i => i.ToReadDto()).ToList();

            return Ok(codeDtos);
        }




        [HttpPut]
        public async Task<IActionResult> EditSystemCode(int id, [FromBody] UpdateSystemCodeDto UpdatedSystemCode)
        {

            var existingCode = await _context.SystemCodes.FindAsync(id);
            if (existingCode == null)
                return NotFound("SystemCode not found.");


            existingCode.Code = UpdatedSystemCode.Code;
            existingCode.Notes = UpdatedSystemCode.Notes;
            existingCode.UpdatedOn = UpdatedSystemCode.UpdatedOn; 

            await _context.SaveChangesAsync();

            return Ok(existingCode.ToReadDto());

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCode(int id)
        {
            var systemCode = await _context.SystemCodes.FindAsync(id);

            if (systemCode == null)
            {
                return NotFound($"SystemCode with ID {id} was not found.");
            }

            _context.SystemCodes.Remove(systemCode);
            await _context.SaveChangesAsync();

            var SystemCodes = await _context.SystemCodes.ToListAsync();

            var allSystemCodes = SystemCodes.Select(p => p.ToReadDto()).ToList();

            return Ok(allSystemCodes);
        }




    }
}
