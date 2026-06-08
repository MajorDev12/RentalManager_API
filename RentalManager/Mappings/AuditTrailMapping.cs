using Microsoft.Identity.Client;
using RentalManager.DTOs.AuditTrail;
using RentalManager.DTOs.Expense;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Mappings
{
    public static class AuditTrailMapping
    {
        public static AuditTrail ToEntity(this CREATEAuditDto dto, int userId, int accountId) => new AuditTrail
        {
            UserId = userId,
            AccountId = accountId,
            EntityId = dto.EntityId,
            EntityName = dto.EntityName,
            Action = dto.Action,
            OldValues = dto.OldValues,
            NewValues = dto.NewValues,
            CreatedAt = DateTime.UtcNow
        };

        public static READAuditTrailDto ToReadDto(this AuditTrail dto) => new READAuditTrailDto
        {
            EntityId = dto.EntityId,
            EntityName = dto.EntityName,
            Action = dto.Action,
            OldValues = dto.OldValues,
            NewValues = dto.NewValues,
            CreatedAt = dto.CreatedAt
        };
    }
}
