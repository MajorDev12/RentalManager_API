using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RentalManager.Authorization.Permissions;
using RentalManager.DTOs.MeterReading;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Services.MeterReadingService;
using RentalManager.Services.UtilityBillService;

namespace RentalManager.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MeterReadingController : BaseController
    {
        private readonly IMeterReadingService _service;

        public MeterReadingController(IMeterReadingService service)
        {
            _service = service;
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Read)]
        [HttpGet("MeterReading/{unitId}/{utilityId}")]
        public async Task<IActionResult> GetReadingForUnit(int unitId, int utilityId)
        {
            var result = await _service.GetPreviousReading(unitId, utilityId);
            return HandleResponse(result);
        }


        [Authorize(Policy = PermissionNames.UtilityBill.Create)]
        [HttpPost("MeterReading")]
        public async Task<IActionResult> AddMeterReading([FromBody] CREATEMeterReading reading)
        {
            var result = await _service.Add(reading);
            return HandleResponse(result);
        }


        [HttpGet("MeterReadings/properties/{propertyId}/utilities")]
        public async Task<IActionResult> GetUtilities(
        int propertyId,
        bool? isMetered)
        {
            var result = await _service.GetAvailableUtilitiesAsync(propertyId, isMetered);

            return HandleResponse(result);
        }


        [HttpGet("MeterReadings/properties/{propertyId}/utilities/{utilityId}/sheet")]
        public async Task<IActionResult> GetRecordingSheet(
            int propertyId,
            int utilityId)
        {
            var result = await _service
                .GetRecordingSheetAsync(propertyId, utilityId);

            return HandleResponse(result);
        }


        [HttpPost("MeterReading/bulk")]
        public async Task<IActionResult> BulkRecord(
            [FromBody] BulkMeterReadingDto dto)
        {
            var result = await _service.BulkRecordReadingsAsync(dto);

            return HandleResponse(result);
        }

    }
}
