using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.UnitTypeService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitTypeController : ControllerBase
    {
        private readonly IUnitTypeService _service;

        public UnitTypeController(IUnitTypeService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetUnitType()
        {
            try
            {
                var types = await _service.GetAll();
                return Ok(types);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitTypeById(int id)
        {
            try
            {
                var type = await _service.GetById(id);
                return Ok(type);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddUnitType([FromBody] CREATEUnitTypeDto AddedUnitType)
        {

            try
            {
                var type = await _service.Add(AddedUnitType);
                return Ok(type);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> EditUnitType(int id, [FromBody] UPDATEUnitTypeDto UpdatedType)
        {
            try
            {
                var type = await _service.Update(id, UpdatedType);
                return Ok(type);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitType(int id)
        {
            try
            {
                var type = await _service.Delete(id);
                return Ok(type);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUnitTypesByProperty(int PropertyId)
        {
            try
            {
                var type = await _service.GetByPropertyId(PropertyId);
                return Ok(type);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
