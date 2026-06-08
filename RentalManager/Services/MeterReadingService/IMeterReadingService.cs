using RentalManager.DTOs.MeterReading;
using RentalManager.DTOs.UtilityBill;

namespace RentalManager.Services.MeterReadingService
{
    public interface IMeterReadingService
    {
        Task GetAll();
        Task<ApiResponse<READMeterReading?>> GetPreviousReading(int unitId, int utilityId);
        Task<ApiResponse<List<READMeterReading>?>> GetAllPreviousReading(int propertyId, int utilityId);
        Task<ApiResponse<READMeterReading>> Add(CREATEMeterReading reading);


        Task<ApiResponse<List<PropertyUtilityDto>>> GetAvailableUtilitiesAsync(
        int propertyId,
        bool? isMetered = null);

        Task<ApiResponse<UtilityRecordingSheetDto>> GetRecordingSheetAsync(
            int propertyId,
            int utilityId);

        Task<ApiResponse<bool>> BulkRecordReadingsAsync(BulkMeterReadingDto dto);
    }
}
