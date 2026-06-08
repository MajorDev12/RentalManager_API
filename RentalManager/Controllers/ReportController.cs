using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Policies;
using RentalManager.DTOs.Report;
using RentalManager.Services.ReportService;

namespace RentalManager.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }


        [Authorize(Policy = PermissionNames.Report.Read)]
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
