using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.UnitService;

namespace RentalManager.Controllers
{
    [Route("api/Units")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _service;

        public UnitController(IUnitService service)
        {
            _service = service;
        }


        [Authorize(Policy = PolicyNames.Unit.Read)]
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


        [Authorize(Policy = PolicyNames.Unit.Read)]
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

        [Authorize(Policy = PolicyNames.Unit.Write)]
        [HttpPost]
        public async Task<IActionResult> AddUnit([FromBody] CREATEUnitDto AddedUnit)
        {
            try
            {
                var result = await _service.Add(AddedUnit);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }



        [Authorize(Policy = PolicyNames.Unit.Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUnit(int id, [FromBody] UPDATEUnitDto UpdatedUnit)
        {

            try
            {
                var result = await _service.Update(id, UpdatedUnit);

                if (result.Success == false) return BadRequest();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

        }


        [Authorize(Policy = PolicyNames.Unit.Delete)]
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



        [Authorize(Policy = PolicyNames.Unit.Read)]
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
