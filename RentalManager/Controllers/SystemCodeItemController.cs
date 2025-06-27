using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.SystemCodeItemService;
using RentalManager.Services.SystemCodeService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeItemController : ControllerBase
    {
        private readonly ISystemCodeItemService _service;

        public SystemCodeItemController(ISystemCodeItemService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<IActionResult> GetSystemCodeItems()
        {
            try
            {
                var items = await _service.GetAll();
                return Ok(items);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetSystemCodeItemById(int id)
        {
            try
            {
                var item = await _service.GetById(id);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSystemCodeItem([FromBody] CREATESystemCodeItemDto AddedItem)
        {
            try
            {
                var item = await _service.Add(AddedItem);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditSystemCodeItem(int id, [FromBody] UPDATESystemCodeItemDto UpdatedItem)
        {

            try
            {
                var item = await _service.Update(id, UpdatedItem);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCodeItem(int id)
        {
            try
            {
                var item = await _service.Delete(id);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        //[HttpGet("BY-NAME/{codeName}")]
        //public async Task<IActionResult> GetSystemCodeItemsByCode(string codeName)
        //{
        //    var systemCode = await _context.SystemCodes
        //        .Include(c => c.SystemCodeItems)
        //        .FirstOrDefaultAsync(c => c.Code == codeName);

        //    if (systemCode == null || !systemCode.SystemCodeItems.Any())
        //    {
        //        return NotFound(new ApiResponse<object>($"No items found for SystemCode '{codeName}'."));
        //    }

        //    var result = systemCode.SystemCodeItems.Select(i => i.ToReadDto()); // use your mapping
        //    return Ok(new ApiResponse<IEnumerable<READSystemCodeItemDto>>(result, "Items fetched successfully."));
        //}




    }
}
