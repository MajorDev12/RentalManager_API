using RentalManager.DTOs.AuditTrail;
using RentalManager.Mappings;
using RentalManager.Repositories.AuditTrailRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.AuditTrailService
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IAuditTrailRepository _repo;
        private readonly ICurrentUser _currentUser;

        public AuditTrailService(IAuditTrailRepository repo, ICurrentUser currentUser) 
        { 
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<READAuditTrailDto>> AddAsync(CREATEAuditDto auditTrail)
        {
            // get audit
            var entity = auditTrail.ToEntity(_currentUser.UserId, _currentUser.AccountId);

            //add
            await _repo.AddAsync(entity);

            return ApiResponse<READAuditTrailDto>.SuccessResponse(entity.ToReadDto(), "Added AuditTrail Successfully");
        }

        public Task<ApiResponse<List<READAuditTrailDto>?>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<READAuditTrailDto?>> GetById(int auditId)
        {
            throw new NotImplementedException();
        }
    }
}
