using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.Tenant;
using RentalManager.Services;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _service;

        public TenantController(ITenantService service)
        {
            _service = service;
        }


        [HttpGet("Tenants")]
        public async Task<IActionResult> GetTenants()
        {
            try
            {
                var result = await _service.GetAll();

                if(result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("Tenants/{id}")]
        public async Task<IActionResult> GetTenantById(int id)
        {
            try
            {
                var result = await _service.GetById(id);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Tenants")]
        public async Task<IActionResult> AddTenant([FromBody] CREATETenantDto AddedTenant)
        {
            try
            {
                var result = await _service.Add(AddedTenant);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpPut("Tenants/{id}")]
        public async Task<IActionResult> EditTenant(int id, [FromBody] UPDATETenantDto updatedTenant)
        {

            try
            {
                var result = await _service.Update(id, updatedTenant);

                if (result.Success == false) return BadRequest(result);   

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpDelete("Tenants/{id}")]
        public async Task<IActionResult> DeleteTenant(int id)
        {
            try
            {
                var result = await _service.Delete(id);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Tenants/AssignUnit")]
        public async Task<IActionResult> AssignUnit([FromBody] ASSIGNUnitDto unitAssigned)
        {
            try
            {
                var result = await _service.AssignUnit(unitAssigned);


                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
