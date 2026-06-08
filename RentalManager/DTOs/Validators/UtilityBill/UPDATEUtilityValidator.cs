using FluentValidation;
using RentalManager.DTOs.UtilityBill;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.UtilityBill
{
    public class UPDATEUtilityValidator
        : AbstractValidator<PATCHUtilityDto>
    {
        public UPDATEUtilityValidator()
        {
            RuleFor(x => x.PropertyId)
                .ValidId()
                .When(x => x.PropertyId != null);

            RuleFor(x => x.UtilityId)
                .ValidId()
                .When(x => x.UtilityId != null);

            RuleFor(x => x.BillingCycleId)
                .ValidId()
                .When(x => x.BillingCycleId != null);

            RuleFor(x => x.UnitId).ValidId().When(x => x.UnitId != null);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .When(x => x.Amount != null);

            RuleFor(x => x.Amount)
                .LessThanOrEqualTo(1_000_000)
                .WithMessage("Amount is too large")
                .When(x => x.Amount != null);

            RuleFor(x => x.Amount)
                .PrecisionScale(18, 2, true)
                .WithMessage("Amount cannot exceed 2 decimal places")
                .When(x => x.Amount != null);

            RuleFor(x => x.IsMetered)
                .Must(x => x == true || x == false)
                .When(x => x.IsMetered != null)
                .WithMessage("Invalid recurring value");

            RuleFor(x => x)
                .Must(x =>
                    x.PropertyId != null ||
                    x.UtilityId != null ||
                    x.BillingCycleId != null ||
                    x.UnitId != null ||
                    x.Amount != null ||
                    x.IsMetered != null)
                .WithMessage("At least one field must be provided for update");
        }
    }
}