using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult HandleResponse<T>(ApiResponse<T> response)
        {
            if (response == null)
                return StatusCode(500, "Internal server error");

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        protected IActionResult HandleCreatedResponse<T>(
            ApiResponse<T> response,
            string actionName,
            object routeValues)
        {
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(actionName, routeValues, response);
        }
    }
}
