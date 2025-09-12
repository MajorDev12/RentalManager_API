using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
