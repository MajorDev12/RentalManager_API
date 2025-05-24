using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SystemCodeItemController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> GetSystemCodeItems()
        {
            var systemCodeItems = await _context.SystemCodeItems.ToListAsync();

            if (!systemCodeItems.Any())
            {
                return NotFound("There are no SystemCodeItems available.");
            }

            var systemCodeItemDtos = systemCodeItems.Select(it => it.ToReadDto()).ToList();

            return Ok(systemCodeItemDtos);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetSystemCodeItemById(int id)
        {
            var item = await _context.SystemCodes.FirstOrDefaultAsync(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item.ToReadDto());
        }


        [HttpPost]
        public async Task<IActionResult> AddSystemCodeItem([FromBody] CREATESystemCodeItemDto AddedItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = AddedItem.ToEntity();

            _context.SystemCodeItems.Add(item);
            await _context.SaveChangesAsync();

            var allItems = await _context.SystemCodeItems.ToListAsync();

            var itemDtos = allItems.Select(i => i.ToReadDto()).ToList();

            return Ok(itemDtos);
        }


        [HttpPut]
        public async Task<IActionResult> EditSystemCodeItem(int id, [FromBody] UPDATESystemCodeItemDto UpdatedItem)
        {

            var existingItem= await _context.SystemCodeItems.FindAsync(id);
            if (existingItem == null)
                return NotFound("SystemCode not found.");


            existingItem.Item = UpdatedItem.Item;
            existingItem.Notes = UpdatedItem.Notes;
            existingItem.UpdatedOn = UpdatedItem.UpdatedOn;

            await _context.SaveChangesAsync();

            return Ok(existingItem.ToReadDto());

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCodeItem(int id)
        {
            var item = await _context.SystemCodeItems.FindAsync(id);

            if (item == null)
            {
                return NotFound($"SystemCode with ID {id} was not found.");
            }

            _context.SystemCodeItems.Remove(item);
            await _context.SaveChangesAsync();

            var allItems = await _context.SystemCodeItems.ToListAsync();

            var allItemsDto = allItems.Select(p => p.ToReadDto()).ToList();

            return Ok(allItemsDto);
        }





    }
}
