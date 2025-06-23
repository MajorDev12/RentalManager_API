using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.SystemCode;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.PropertyService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PropertyController : Controller
    {

        private readonly IPropertyService _PropertyContext;

        public PropertyController(IPropertyService context)
        {
            _PropertyContext = context;
        }

        [HttpGet("Properties")]
        public async Task<ActionResult<IEnumerable<READPropertyDto>>> GetProperties()
        {
            
            try
            {
                var properties = await _PropertyContext.GetAll();
                return Ok(properties);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("Properties/{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            try
            {
                var property = await _PropertyContext.GetById(id);
                return Ok(property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }





        [HttpPost("Property")]
        public async Task<IActionResult> AddProperty([FromBody] CREATEPropertyDto AddedProperty)
        {

            try
            {
                var property = await _PropertyContext.Create(AddedProperty);
                return Ok(property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        [HttpPut("Property/{id}")]
        public async Task<IActionResult> EditProperty(int id, [FromBody] UPDATEPropertyDto dto)
        {

            try
            {
                var property = await _PropertyContext.Update(dto);
                return Ok(property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpDelete("Property/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                var property = await _PropertyContext.Delete(id);
                return Ok(property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






    }
}
