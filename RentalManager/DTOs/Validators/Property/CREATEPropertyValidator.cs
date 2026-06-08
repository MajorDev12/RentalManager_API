using FluentValidation;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.Property
{
    public class CREATEPropertyValidator : AbstractValidator<CREATEPropertyDto>
    {
        public CREATEPropertyValidator()
        {
            // ===== PROPERTY RULES =====
            RuleFor(x => x.Name).RequiredName().MaxLength100();

            RuleFor(x => x.EmailAddress).Email();

            RuleFor(x => x.MobileNumber).Phone();

            RuleFor(x => x.PhysicalAddress).RequiredAddress();

            RuleFor(x => x.Country).RequiredLocation();

            RuleFor(x => x.County).RequiredLocation();

            RuleFor(x => x.Area).RequiredLocation();

            RuleFor(x => x.PropertyTypeId).ValidId();


            When(x => x.Utilities != null && x.Utilities.Any(), () =>
            {
                RuleFor(x => x.Utilities)
                    .Must(u => u.Count <= 10)
                    .WithMessage("A maximum of 10 utilities can be added");

                RuleForEach(x => x.Utilities).ChildRules(util =>
                {
                    util.RuleFor(u => u.UtilityId)
                        .ValidId();

                    util.RuleFor(u => u.BillingCycleId)
                        .ValidId();

                    util.RuleFor(u => u.Amount)
                        .GreaterThan(0)
                        .WithMessage("Utility amount must be greater than 0");

                    util.RuleFor(u => u.IsMetered)
                        .NotNull();
                });

                RuleFor(x => x.Utilities)
                    .Must(list => list
                        .GroupBy(x => x.UtilityId)
                        .All(g => g.Count() == 1))
                    .WithMessage("Duplicate utilities are not allowed for a property");
            });
        }
    }
}