using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.Expense;
using RentalManager.Services.ExpenseService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly IExpenseService _service;
        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        [Authorize(Policy = PolicyNames.Expense.Read)]
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
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


        [Authorize(Policy = PolicyNames.Expense.Read)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
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



        [Authorize(Policy = PolicyNames.Expense.Create)]
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] CREATEExpenseDto expense)
        {
            try
            {
                var result = await _service.Add(expense);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Policy = PolicyNames.Expense.Update)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditExpense(int id, [FromBody] UPDATEExpenseDto updatedExpense)
        {

            try
            {
                var result = await _service.Update(id, updatedExpense);

                if (result.Success == false) return BadRequest(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [Authorize(Policy = PolicyNames.Expense.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
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
    }
}
