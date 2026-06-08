using RentalManager.Data;
using RentalManager.DTOs.AuditTrail;
using RentalManager.Models;

namespace RentalManager.Repositories.AuditTrailRepository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditTrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<AuditTrail> AddAsync(AuditTrail auditTrail)
        {
            _context.Add(auditTrail);
            await _context.SaveChangesAsync();
            return auditTrail;
        }

        public Task<List<AuditTrail>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuditTrail?> GetById(int auditId)
        {
            throw new NotImplementedException();
        }
    }
}
