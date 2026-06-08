using FluentValidation;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.Unit
{
    public class PATCHUnitValidator : AbstractValidator<PATCHUnitDto>
    {
        public PATCHUnitValidator() 
        {
            RuleFor(u => u.PropertyId).ValidId().When(x => x.PropertyId > 0);
            RuleFor(u => u.UnitTypeId).ValidId().When(x => x.UnitTypeId > 0);
            RuleFor(u => u.RentalTypeId).ValidId().When(x => x.RentalTypeId > 0);
            RuleFor(u => u.BillingCycleId).ValidId().When(x => x.BillingCycleId > 0);
            RuleFor(u => u.Floor).ValidId().When(x => x.Floor > 0);
            RuleFor(u => u.Amount).ValidAmount().When(x => x.Amount > 0);
            RuleFor(u => u.Name).RequiredName().MaxLength100().When(x => x.Name != null);

            RuleFor(x => x)
                .Must(x =>
                    x.PropertyId != null ||
                    x.UnitTypeId != null ||
                    x.RentalTypeId != null ||
                    x.BillingCycleId != null ||
                    x.Floor != null ||
                    x.Amount != null ||
                    x.Name != null)
                .WithMessage("At least one field must be provided for update");
        }
    }
}
