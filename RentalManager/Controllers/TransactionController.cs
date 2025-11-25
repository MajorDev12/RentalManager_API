using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.Transaction;
using RentalManager.Services.TransactionService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var result = await _service.GetAll();

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] CREATETransactionDto transaction)
        {
            try
            {
                var result = await _service.Add(transaction);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] CREATEPaymentDto payment)
        {
            try
            {
                var result = await _service.AddPayment(payment);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("AddInvoice")]
        public async Task<IActionResult> AddInvoiceCharge([FromBody] CREATEIncoiceTransactionDto transaction)
        {
            try
            {
                var result = await _service.AddCharge(transaction);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> EditTransaction(int id, [FromBody] UPDATETransactionDto updatedTransaction)
        {

            try
            {
                var result = await _service.Update(id, updatedTransaction);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id) 
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


        [HttpGet("UnpaidTenants")]
        public async Task<IActionResult> UnpaidTenants()
        {
            try
            {
                var result = await _service.GetRentBalances();

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("TenantBalances")]
        public async Task<IActionResult> TenantBalances(int userId)
        {
            try
            {
                var result = await _service.GetUserBalances(userId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GenerateRentInvoices")]
        public async Task<IActionResult> GenerateRentInvoices(int propertyId)
        {
            try
            {
                var result = await _service.GenerateRentInvoices(propertyId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("GenerateUtilityBillInvoices")]
        public async Task<IActionResult> GenerateUtilityInvoices(int propertyId)
        {
            try
            {
                var result = await _service.GenerateUtilityBillInvoices(propertyId);

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
