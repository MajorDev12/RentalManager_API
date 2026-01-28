using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Authorization.Policies;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.SystemCode;
using RentalManager.Helpers.Authorization;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.PropertyService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class PropertyController : Controller
    {

        private readonly IPropertyService _PropertyContext;

        public PropertyController(IPropertyService context)
        {
            _PropertyContext = context;
        }

        [Authorize(Policy = PolicyNames.Property.Read)]
        [HttpGet("Properties")]
        public async Task<ActionResult<IEnumerable<READPropertyDto>>> GetProperties()
        {
            
            try
            {
                var result = await _PropertyContext.GetAll();
                
                if(result.Success == false) return BadRequest();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }




        [HttpGet("Properties/{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            try
            {
                var result = await _PropertyContext.GetById(id);

                if (result.Success == false) return BadRequest();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            
        }





        [HttpPost("Property")]
        public async Task<IActionResult> AddProperty([FromBody] CREATEPropertyDto AddedProperty)
        {

            try
            {
                var accountId = User.AccountId();
                var result = await _PropertyContext.Create(AddedProperty, accountId);

                if (result.Success == false) return BadRequest();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }






        [HttpPut("Property/{id}")]
        public async Task<IActionResult> EditProperty(int id, [FromBody] UPDATEPropertyDto dto)
        {

            try
            {
                var result = await _PropertyContext.Update(id, dto);

                if (result.Success == false) return BadRequest();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

        }



        [HttpDelete("Property/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _PropertyContext.Delete(id);

                if (result.Success == false) return BadRequest();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }






    }
}
