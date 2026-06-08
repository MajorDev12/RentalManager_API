using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.UnitType;
using RentalManager.Services.UnitTypeService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UnitTypeController : ControllerBase
    {
        private readonly IUnitTypeService _service;

        public UnitTypeController(IUnitTypeService service)
        {
            _service = service;
        }


        [Authorize(Policy = PermissionNames.UnitType.Read)]
        [HttpGet("UnitTypes")]
        public async Task<IActionResult> GetUnitType()
        {
            try
            {
                var result = await _service.GetAll();

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.UnitType.Read)]
        [HttpGet("UnitTypes/{id}")]
        public async Task<IActionResult> GetUnitTypeById(int id)
        {
            try
            {
                var result = await _service.GetById(id);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }



        [Authorize(Policy = PermissionNames.UnitType.Read)]
        [HttpPost("UnitType")]
        public async Task<IActionResult> AddUnitType([FromBody] CREATEUnitTypeDto AddedUnitType)
        {

            try
            {
                var result = await _service.Add(AddedUnitType);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.UnitType.Update)]
        [HttpPut("UnitType/{id}")]
        public async Task<IActionResult> EditUnitType(int id, [FromBody] UPDATEUnitTypeDto UpdatedType)
        {
            try
            {
                var result = await _service.Update(id, UpdatedType);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

        }

        [Authorize(Policy = PermissionNames.UnitType.Delete)]
        [HttpDelete("UnitType/{id}")]
        public async Task<IActionResult> DeleteUnitType(int id)
        {
            try
            {
                var result = await _service.Delete(id);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.UnitType.Read)]
        [HttpGet("UnitTypes/By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUnitTypesByProperty(int PropertyId)
        {
            try
            {
                var result = await _service.GetByPropertyId(PropertyId);

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
