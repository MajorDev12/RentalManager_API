using FluentValidation;
using RentalManager.DTOs.UtilityBill;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.UtilityBill
{
    public class CREATEUtilityValidator
        : AbstractValidator<CREATEUtilityBillDto>
    {
        public CREATEUtilityValidator() 
        {
            RuleFor(u => u.PropertyId).ValidId();
            RuleFor(u => u.UtilityId).ValidId();
            RuleFor(u => u.BillingCycleId).ValidId();
            RuleFor(x => x.UnitId).ValidId().When(x => x.UnitId != null);

            RuleFor(x => x.IsMetered)
                .Must(x => x == true || x == false)
                .WithMessage("Invalid recurring value");

            RuleFor(u => u.Amount).ValidAmount();

        }
    }
}
