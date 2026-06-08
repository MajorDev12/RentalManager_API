using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.DTOs.Property;
using RentalManager.Services.PropertyService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class PropertyController : BaseController
    {

        private readonly IPropertyService _PropertyContext;

        public PropertyController(IPropertyService context)
        {
            _PropertyContext = context;
        }

        [Authorize(Policy = PermissionNames.Property.Read)]
        [HttpGet("Properties")]
        public async Task<IActionResult> GetProperties([FromQuery] PropertyQueryFilter filter)
        {
            var result = await _PropertyContext.GetFiltered(filter);

            return HandleResponse(result);

        }

        [Authorize(Policy = PermissionNames.Property.Read)]
        [HttpGet("Lookups/Properties")]
        public async Task<IActionResult> GetLookupProperties()
        {
            var result = await _PropertyContext.GetAllLookups();

            return HandleResponse(result);

        }



        [Authorize(Policy = PermissionNames.Property.Read)]
        [HttpGet("Properties/{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            try
            {
                var result = await _PropertyContext.GetById(id);

                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }




        [Authorize(Policy = PermissionNames.Property.Create)]
        [HttpPost("Property")]
        public async Task<IActionResult> AddProperty([FromBody] CREATEPropertyDto AddedProperty)
        {


            var result = await _PropertyContext.Create(AddedProperty);

            return HandleCreatedResponse(
                result,
                nameof(GetPropertyById),
                new { id = result.Data?.Id }
            );

        }





        [Authorize(Policy = PermissionNames.Property.Update)]
        [HttpPatch("Property/{id}")]
        public async Task<IActionResult> EditProperty(int id, [FromBody] PATCHPropertyDto dto)
        {

            try
            {
                var result = await _PropertyContext.Update(id, dto);

                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize(Policy = PermissionNames.Property.Delete)]
        [HttpDelete("Property/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _PropertyContext.Delete(id);

                if (!result.Success)
                    return BadRequest(result);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






    }
}
