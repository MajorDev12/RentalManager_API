using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.MeterReading;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;
using RentalManager.Repositories.MeterReadingRepository;
using RentalManager.Repositories.UtilityBillRepository;

namespace RentalManager.Services.MeterReadingService
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeterReadingRepository _repo;
        private readonly IUtilityBillRepository _utilityRepo;

        public MeterReadingService(IMeterReadingRepository repo,
            IUtilityBillRepository utilityRepo,
            ApplicationDbContext context)
        {
            _context = context;
            _repo = repo;
            _utilityRepo = utilityRepo;
        }


        public async Task<ApiResponse<READMeterReading>> Add(CREATEMeterReading reading)
        {
            var verifyUtility = await _utilityRepo.ExistAsync(reading.UtilityBillId);
            var utilitySelected = await _utilityRepo.GetByIdAsync(reading.UtilityBillId);

            if (!verifyUtility || utilitySelected == null)
            {
                return ApiResponse<READMeterReading>
                    .FailResponse("Utility Selected Does Not Exist");
            }

            var lastReading = await _repo.GetLastReadingAsync(
                reading.UnitId,
                reading.UtilityBillId
            );

            // Use last current reading as previous reading if available
            decimal previousReading = lastReading != null
                ? lastReading.CurrentReading
                : reading.PreviousReading;

            var unitsConsumed = reading.CurrentReading - previousReading;

            var addReading = new MeterReading
            {
                UtilityBillId = reading.UtilityBillId,
                PreviousReading = previousReading,
                CurrentReading = reading.CurrentReading,
                ReadingDate = DateTime.UtcNow,
                UnitsConsumed = unitsConsumed,
                Rate = utilitySelected.Amount,
                TotalAmount = unitsConsumed * utilitySelected.Amount,
            };

            await _repo.AddReadingAsync(addReading);

            var response = new READMeterReading
            {
                UtilityBillId = addReading.UtilityBillId,
                PreviousReading = addReading.PreviousReading,
                CurrentReading = addReading.CurrentReading,
            };

            return ApiResponse<READMeterReading>
                .SuccessResponse(response, "Meter Reading Added Successfully");
        }

        public Task GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<READMeterReading>?>> GetAllPreviousReading(int propertyId, int utilityId)
        {

            // 






            //return ApiResponse<List<READMeterReading>?>.SuccessResponse(null, "Fetched Successfuly");
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<READMeterReading?>> GetPreviousReading(int unitId, int utilityId)
        {
            var verify = await _utilityRepo.VerifyUtilityByUnit(unitId, utilityId);

            if (verify)
                return ApiResponse<READMeterReading?>.FailResponse("Unit doesnt have the selected utility");

            var reading = await _repo.GetPreviousReadingAsync(unitId, utilityId);

            if (reading == null)
                return ApiResponse<READMeterReading?>.FailResponse("No Previous Reading Found, Set to 0.0");


            var readingDto = new READMeterReading
            {
                UtilityBillId = reading.UtilityBillId,
                PreviousReading = reading.PreviousReading,
                CurrentReading = reading.CurrentReading,
            };

            return ApiResponse<READMeterReading?>.SuccessResponse(readingDto, "Fetched Successfuly");
        }






        public async Task<ApiResponse<List<PropertyUtilityDto>>> GetAvailableUtilitiesAsync(
        int propertyId,
        bool? isMetered = null)
        {
            var data = await _utilityRepo.GetAvailableUtilitiesAsync(propertyId, isMetered);

            if (data == null || !data.Any())
                return ApiResponse<List<PropertyUtilityDto>>.SuccessResponse(new List<PropertyUtilityDto>(), "No Data Found");

            return ApiResponse<List<PropertyUtilityDto>>.SuccessResponse(data, "Successfully Retrieved Data");
        }


        public async Task<ApiResponse<UtilityRecordingSheetDto>> GetRecordingSheetAsync(
            int propertyId,
            int utilityId)
        {
            try
            {
                var configurations = await _utilityRepo
                    .GetUtilityConfigurationsAsync(propertyId, utilityId);

                if (!configurations.Any())
                {
                    return ApiResponse<UtilityRecordingSheetDto>
                        .FailResponse("Utility configuration not found.");
                }

                var propertyWideConfig = configurations
                    .FirstOrDefault(x => x.UnitId == null);

                var unitSpecificConfigs = configurations
                    .Where(x => x.UnitId != null)
                    .ToList();

                List<Unit> participatingUnits;

                // CASE A: PROPERTY-WIDE UTILITY
                if (propertyWideConfig != null)
                {
                    participatingUnits = await _context.Units
                        .Where(x => x.PropertyId == propertyId)
                        .ToListAsync();
                }
                else
                {
                    // CASE B: UNIT-SPECIFIC ONLY
                    var unitIds = unitSpecificConfigs
                        .Select(x => x.UnitId!.Value)
                        .Distinct()
                        .ToList();

                    participatingUnits = await _context.Units
                        .Where(x => unitIds.Contains(x.Id))
                        .ToListAsync();
                }

                var rows = new List<UtilityRecordingUnitDto>();

                foreach (var unit in participatingUnits)
                {
                    // CHECK UNIT OVERRIDE FIRST
                    var overrideConfig = unitSpecificConfigs
                        .FirstOrDefault(x => x.UnitId == unit.Id);

                    // USE OVERRIDE OR FALLBACK TO PROPERTY-WIDE CONFIG
                    var appliedConfig = overrideConfig ?? propertyWideConfig;

                    if (appliedConfig == null)
                    {
                        continue;
                    }

                    var previousReading = appliedConfig.UtilityReadings
                        .OrderByDescending(x => x.ReadingDate)
                        .FirstOrDefault();

                    rows.Add(new UtilityRecordingUnitDto
                    {
                        UtilityBillId = appliedConfig.Id,
                        UnitId = unit.Id,
                        UnitName = unit.Name,

                        PreviousReading = previousReading?.CurrentReading ?? 0,

                        Rate = appliedConfig.Amount,

                        IsOverride = overrideConfig != null
                    });
                }

                var data = new UtilityRecordingSheetDto
                {
                    UtilityId = utilityId,
                    UtilityName = configurations.First().Utility.Item,

                    IsPropertyWide = propertyWideConfig != null,

                    Units = rows
                };

                return ApiResponse<UtilityRecordingSheetDto>
                    .SuccessResponse(data, "Successfully retrieved data.");
            }
            catch (Exception ex)
            {
                return ApiResponse<UtilityRecordingSheetDto>
                    .FailResponse($"An error occurred while retrieving recording sheet: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> BulkRecordReadingsAsync(BulkMeterReadingDto dto)
        {
            try
            {
                foreach (var item in dto.Readings)
                {
                    var utilityBill = await _context.UtilityBills
                        .Include(x => x.UtilityReadings)
                        .FirstOrDefaultAsync(x => x.Id == item.UtilityBillId);


                    if (utilityBill == null)
                    {
                        return ApiResponse<bool>.FailResponse("Utility configuration not found.");
                    }

                    var previousReading = utilityBill.UtilityReadings
                        .OrderByDescending(x => x.ReadingDate)
                        .FirstOrDefault();


                    decimal previous = previousReading?.CurrentReading ?? 0;
                    decimal consumed = item.CurrentReading - previous;

                    if (consumed < 0)
                    {
                        return ApiResponse<bool>.FailResponse("Current reading cannot be less than previous reading.");
                    }


                    decimal totalAmount = consumed * utilityBill.Amount;
                    var reading = new MeterReading
                    {
                        UtilityBillId = utilityBill.Id,

                        PreviousReading = previous,
                        CurrentReading = item.CurrentReading,
                        UnitsConsumed = consumed,

                        Rate = utilityBill.Amount,
                        TotalAmount = totalAmount,

                        ReadingDate = DateTime.UtcNow
                    };

                    _context.MeterReadings.Add(reading);
                    await _context.SaveChangesAsync();
                }

                return ApiResponse<bool>.SuccessResponse(true, "Readings Added Successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Error Occurred: {ex.Message}");
            }
        }
    }
}
