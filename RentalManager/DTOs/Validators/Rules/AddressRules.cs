using FluentValidation;

namespace RentalManager.DTOs.Validators.Rules
{
    public static class AddressRules
    {
        public static IRuleBuilderOptions<T, string> RequiredAddress<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(200);
        }

        public static IRuleBuilderOptions<T, string> RequiredLocation<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Location is required")
                .MaximumLength(100);
        }
    }
}