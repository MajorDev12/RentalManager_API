using RentalManager.DTOs.AuditTrail;
using RentalManager.Models;

namespace RentalManager.Services.AuditTrailService
{
    public interface IAuditTrailService
    {
        Task<ApiResponse<List<READAuditTrailDto>?>> GetAllAsync();
        Task<ApiResponse<READAuditTrailDto?>> GetById(int auditId);

        Task<ApiResponse<READAuditTrailDto>> AddAsync(CREATEAuditDto auditTrail);
    }
}
