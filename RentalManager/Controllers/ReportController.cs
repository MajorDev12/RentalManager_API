using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.Report;
using RentalManager.Services.ReportService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }


        [HttpGet("IncomeSummary")]
        public async Task<IActionResult> GetSummary([FromQuery] ReportFilterDto filter)
        {
            try
            {
                var result = await _service.GetReport(filter);

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
