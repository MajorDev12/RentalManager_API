using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Unit;
using RentalManager.Services.UnitService;

namespace RentalManager.Controllers
{
    [Route("api/Units")]
    [ApiController]
    public class UnitController : BaseController
    {
        private readonly IUnitService _service;

        public UnitController(IUnitService service)
        {
            _service = service;
        }


        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet]
        public async Task<IActionResult> GetUnits()
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



        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet("Lookups")]
        public async Task<IActionResult> GetUnitLookups()
        {
            var result = await _service.GetLookups();

            return HandleResponse(result);
        }



        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet("Filtered")]
        public async Task<IActionResult> GetUnitsFiltered([FromQuery] UnitQueryFilter filter)
        {
            var result = await _service.GetFiltered(filter);

            return HandleResponse(result);

        }


        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById(int id)
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


        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet("Vacants")]
        public async Task<IActionResult> GetVacantUnits()
        {
            try
            {
                var result = await _service.GetVacants();

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }



        [Authorize(Policy = PermissionNames.Unit.Create)]
        [HttpPost]
        public async Task<IActionResult> AddUnit([FromBody] CREATEUnitDto AddedUnit)
        {
            var result = await _service.Add(AddedUnit);
            return HandleResponse(result);
        }



        [Authorize(Policy = PermissionNames.Unit.Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUnit(int id, [FromBody] UPDATEUnitDto UpdatedUnit)
        {
            var result = await _service.Update(id, UpdatedUnit);
            return HandleResponse(result);
        }



        [Authorize(Policy = PermissionNames.Unit.Update)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUnit(int id, [FromBody] PATCHUnitDto PatchedUnit)
        {
            var result = await _service.Patch(id, PatchedUnit);
            return HandleResponse(result);
        }


        [Authorize(Policy = PermissionNames.Unit.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
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



        [Authorize(Policy = PermissionNames.Unit.Read)]
        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUnitsByProperty(int PropertyId)
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
