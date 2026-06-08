using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.Transaction;
using RentalManager.Services.TransactionService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [Authorize(Policy = PermissionNames.Transaction.ReadAll)]
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
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.Transaction.ReadAll)]
        [HttpGet("By-User/{userId}")]
        public async Task<IActionResult> GetTransactionsByUser(int domainUserId)
        {
            try
            {
                var result = await _service.GetByUser(domainUserId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.Transaction.ReadAll)]
        [HttpGet("By-Tenant/{TenantId}")]
        public async Task<IActionResult> GetTransactionsByTenant(int tenantId)
        {
            try
            {
                var result = await _service.GetByTenantId(tenantId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.Transaction.Create)]
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



        [Authorize(Policy = PermissionNames.Transaction.Create)]
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



        [Authorize(Policy = PermissionNames.Transaction.Create)]
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


        [Authorize(Policy = PermissionNames.Transaction.Update)]
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


        [Authorize(Policy = PermissionNames.Transaction.Delete)]
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


        [Authorize(Policy = PermissionNames.Transaction.ReadAll)]
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


        [Authorize(Policy = PermissionNames.Transaction.ReadSelf)]
        [HttpGet("TenantBalances/{tenantId}")]
        public async Task<IActionResult> TenantBalances(int tenantId)
        {
            try
            {
                var result = await _service.GetUserBalances(tenantId);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = PermissionNames.Transaction.Create)]
        [HttpPost("GenerateRentInvoices/{propertyId}")]
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


        [Authorize(Policy = PermissionNames.Transaction.Create)]
        [HttpPost("GenerateUtilityBillInvoices/{propertyId}")]
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
