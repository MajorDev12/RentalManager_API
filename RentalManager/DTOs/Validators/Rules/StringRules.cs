using FluentValidation;

namespace RentalManager.DTOs.Validators.Rules
{
    public static class StringRules
    {
        public static IRuleBuilderOptions<T, string> RequiredName<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50);
        }

        public static IRuleBuilderOptions<T, string> MaxLength100<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.MaximumLength(100);
        }

        public static IRuleBuilderOptions<T, string> MaxLength200<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.MaximumLength(200);
        }
    }
}
