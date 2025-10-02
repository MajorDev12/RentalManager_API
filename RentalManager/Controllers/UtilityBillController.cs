using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Services.UtilityBillService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityBillController : ControllerBase
    {
        private readonly IUtilityBillService _service;

        public UtilityBillController(IUtilityBillService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetUtilityBills()
        {
            try
            {
                var bills = await _service.GetAll();
                return Ok(bills);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
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


        [HttpPost]
        public async Task<IActionResult> AddUtilityBill([FromBody] CREATEUtilityBillDto AddedCharge)
        {
            try
            {
                var bill = await _service.Add(AddedCharge);
                return Ok(bill);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> EditUtilityBill(int id, [FromBody] UPDATEUtilityBillDto UpdatedBill)
        {
            try
            {
                var bill = await _service.Update(id, UpdatedBill);
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
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



        [HttpGet("By-Property/{PropertyId}")]
        public async Task<IActionResult> GetUtilityBillsByProperty(int PropertyId)
        {
            try
            {
                var bills = await _service.GetByPropertyId(PropertyId);
                return Ok(bills);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("By-TenantId/{TenantId}")]
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
