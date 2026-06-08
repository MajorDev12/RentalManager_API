using RentalManager.Models;
using RentalManager.DTOs.AuditTrail;

namespace RentalManager.Repositories.AuditTrailRepository
{
    public interface IAuditTrailRepository
    {
        Task<List<AuditTrail>?> GetAllAsync();
        Task<AuditTrail?> GetById(int auditId);

        Task<AuditTrail> AddAsync(AuditTrail auditTrail);
    }
}
