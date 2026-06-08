using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.UtilityBillService;
using System.Security.Cryptography;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UtilityBillController : BaseController
    {
        private readonly IUtilityBillService _service;

        public UtilityBillController(IUtilityBillService service)
        {
            _service = service;
        }
        

        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBills")]
        public async Task<IActionResult> GetUtilityBills([FromQuery] UtilityBillQueryFilter filter)
        {
            var result = await _service.GetFiltered(filter);
            return HandleResponse(result);
        }



        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBill/Lookups")]
        public async Task<IActionResult> GetLookups()
        {
            var result = await _service.GetAllLookups();
            return HandleResponse(result);
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBill/{id}")]
        public async Task<IActionResult> GetUtilityBillById(int id)
        {
            try
            {
                var bill = await _service.GetById(id);
                return Ok(bill);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PermissionNames.UtilityBill.Create)]
        [HttpPost("UtilityBill")]
        public async Task<IActionResult> AddUtilityBill([FromBody] CREATEUtilityBillDto AddedCharge)
        {
            var result = await _service.Add(AddedCharge);

            return Ok(result);
        }



        [Authorize(Policy = PermissionNames.UtilityBill.Update)]
        [HttpPatch("UtilityBill/{id}")]
        public async Task<IActionResult> EditUtilityBill(int id, [FromBody] PATCHUtilityDto UpdatedBill)
        {
            try
            {
                var bill = await _service.Patch(id, UpdatedBill);
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Policy = PermissionNames.UtilityBill.Delete)]
        [HttpDelete("UtilityBill/{id}")]
        public async Task<IActionResult> DeleteUtilityBill(int id)
        {
            try
            {
                var bill = await _service.Delete(id);
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBill/By-Property/{propertyId}")]
        public async Task<IActionResult> GetUtilityBillsByProperty(
            int propertyId,
            [FromQuery] bool? isMetered = null
        )
        {
            var bills = await _service.GetByPropertyId(propertyId, isMetered);
            return HandleResponse(bills);
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBill/By-Unit/{unitId}")]
        public async Task<IActionResult> GetUtilityBillsByUnit(
            int unitId,
            [FromQuery] bool? isMetered = null
        )
        {
            var bills = await _service.GetByUnitId(unitId, isMetered);
            return HandleResponse(bills);
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("UtilityBill/By-TenantId/{TenantId}")]
        public async Task<IActionResult> GetUtilityBillsByTenant(int TenantId)
        {
            try
            {
                var bills = await _service.GetByTenantId(TenantId);
                return Ok(bills);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
