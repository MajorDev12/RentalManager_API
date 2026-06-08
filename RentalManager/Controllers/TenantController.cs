using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
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

        [Authorize(Policy = PermissionNames.Tenant.ReadAll)]
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

        [Authorize(Policy = PermissionNames.Tenant.ReadSelf)]
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



        [Authorize(Policy = PermissionNames.Tenant.Create)]
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




        [Authorize(Policy = PermissionNames.Tenant.Update)]
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



        [Authorize(Policy = PermissionNames.Tenant.Delete)]
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



        [Authorize(Policy = PermissionNames.Tenant.Assign)]
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




        [Authorize(Policy = PermissionNames.Tenant.Assign)]
        [HttpPost("Tenants/AssignStatus")]
        public async Task<IActionResult> AssignStatus([FromBody] ASSIGNStatusDto status)
        {
            try
            {
                var result = await _service.AssignStatus(status);

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
