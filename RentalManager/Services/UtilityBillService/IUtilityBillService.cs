using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;

namespace RentalManager.Services.UtilityBillService
{
    public interface IUtilityBillService
    {
        Task<ApiResponse<List<READUtilityBillDto>>> GetAll();
        Task<ApiResponse<READUtilityBillDto>> GetById(int id);
        Task<ApiResponse<List<READUtilityBillDto>>> GetByPropertyId(int id);
        Task<ApiResponse<List<READUtilityBillDto>>> GetByTenantId(int id);
        Task<ApiResponse<READUtilityBillDto>> Add(CREATEUtilityBillDto bill);
        Task<ApiResponse<READUtilityBillDto>> Update(int id, UPDATEUtilityBillDto bill);
        Task<ApiResponse<READUtilityBillDto>> Delete(int id);
    }
}
