using FluentValidation;
using RentalManager.DTOs.Unit;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.Unit
{
    public class CREATEUnitValidator : AbstractValidator<CREATEUnitDto>
    {
        public CREATEUnitValidator() 
        {
            RuleFor(u => u.PropertyId).ValidId();
            RuleFor(u => u.UnitTypeId).ValidId();
            RuleFor(u => u.RentalTypeId).ValidId();
            RuleFor(u => u.BillingCycleId).ValidId();
            RuleFor(u => u.Floor).GreaterThanOrEqualTo(0).WithMessage("Floor Number Is Invalid");
            RuleFor(u => u.Amount).ValidAmount();
            RuleFor(u => u.Name).RequiredName().MaxLength100();
        }
    }
}
