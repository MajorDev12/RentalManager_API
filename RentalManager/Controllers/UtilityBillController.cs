using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityBillController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilityBillController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetUtilityBills()
        {
            var charges = await _context.UnitCharges
                .Include(ub => ub.Property)
                .ToListAsync();

            if (!charges.Any())
            {
                return NotFound(new ApiResponse<object>("There are no Utility Bills available."));
            }

            var chargeDtos = charges.Select(it => it.ToReadDto()).ToList();
            return Ok(new ApiResponse<List<READUtilityBillDto>>(chargeDtos, ""));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUtilityBillById(int id)
        {
            var charge = await _context.UnitCharges
                .Include(c => c.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (charge == null)
            {
                return NotFound(new ApiResponse<object>("There is no such data"));
            }

            return Ok(new ApiResponse<READUtilityBillDto>(charge.ToReadDto(), ""));
        }


        [HttpPost]
        public async Task<IActionResult> AddUtilityBill([FromBody] CREATEUtilityBillDto AddedCharge)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>("Validation failed.", errors));
            }

            var charge = AddedCharge.ToEntity();

            _context.UnitCharges.Add(charge);
            await _context.SaveChangesAsync();


            var addedCharge = await _context.UnitCharges
                .Include(c => c.Property)
                .FirstOrDefaultAsync(pr => pr.Id == charge.Id);

            if (addedCharge == null)
            {
                return NotFound(new ApiResponse<object>("Something went wrong while saving"));
            }


            return Ok(new ApiResponse<READUtilityBillDto>(addedCharge.ToReadDto(), ""));
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> EditUtilityBill(int id, [FromBody] UPDATEUtilityBillDto dto)
        {

            var charge = await _context.UnitCharges
                .Include(c => c.Property)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (charge == null)
                return NotFound(new ApiResponse<object>("Utility Bill not found."));


            // Manual update
            charge.Name = dto.Name;
            charge.Amount = dto.Amount;
            charge.PropertyId = dto.PropertyId;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<READUtilityBillDto>(charge.ToReadDto(), ""));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilityBill(int id)
        {
            var charge = await _context.UnitCharges.FindAsync(id);

            if (charge == null)
            {
                return NotFound($"Utility Bill with ID {id} was not found.");
            }

            _context.UnitCharges.Remove(charge);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(charge, "Deleted Successfully"));
        }



        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUtilityBillsByProperty(int PropertyId)
        {
            var charges = await _context.UnitCharges
                .Include(u => u.Property)
                .Where(u => u.PropertyId == PropertyId)
                .ToListAsync();

            if (charges == null || !charges.Any())
            {
                return NotFound(new ApiResponse<object>(null!, "There are no Charges for the specified property."));
            }

            var chargeDtos = charges.Select(u => u.ToReadDto()).ToList();

            return Ok(new ApiResponse<List<READUtilityBillDto>>(chargeDtos, "Charges fetched successfully."));
        }


    }
}
