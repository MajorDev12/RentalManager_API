using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.Home;
using RentalManager.Services.HomeService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _service;
        public HomeController(IHomeService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetDashboradSummary([FromQuery] HomeFilterDto filters)
        {
            try
            {
                var result = await _service.GetSummary(filters);

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
