using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.UnitService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _service;

        public UnitController(IUnitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            try
            {
                var units = await _service.GetAll();
                return Ok(units);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById(int id)
        {
            try
            {
                var unit = await _service.GetById(id);
                return Ok(unit);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUnit([FromBody] CREATEUnitDto AddedUnit)
        {
            try
            {
                var unit = await _service.Add(AddedUnit);
                return Ok(unit);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> EditUnit(int id, [FromBody] UPDATEUnitDto UpdatedUnit)
        {

            try
            {
                var unit = await _service.Update(id, UpdatedUnit);
                return Ok(unit);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            try
            {
                var unit = await _service.Delete(id);
                return Ok(unit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUnitsByProperty(int PropertyId)
        {
            try
            {
                var units = await _service.GetByPropertyId(PropertyId);
                return Ok(units);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
