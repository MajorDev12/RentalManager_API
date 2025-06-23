using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.SystemCode;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.SystemCodeRepository;
using RentalManager.Services.SystemCodeService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
        private readonly ISystemCodeService _service;

        public SystemCodeController (ISystemCodeService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<READSystemCodeDto>>> GetSystemCodes()
        {

            try
            {
                var codes = await _service.GetAll();
                return Ok(codes);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetSystemCodeById(int id)
        {
            try
            {
                var code = await _service.GetById(id);
                return Ok(code);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSystemCode([FromBody] CREATESystemCodeDto AddedSystemCode)
        {
            try
            {
                var code = await _service.Add(AddedSystemCode);
                return Ok(code);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> EditSystemCode(int id, [FromBody] UPDATESystemCodeDto UpdatedSystemCode)
        {

            try
            {
                var codes = await _service.Update(id, UpdatedSystemCode);
                return Ok(codes);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCode(int id)
        {
            try
            {
                var codes = await _service.Delete(id);
                return Ok(codes);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
