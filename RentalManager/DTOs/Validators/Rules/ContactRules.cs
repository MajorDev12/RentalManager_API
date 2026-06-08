using FluentValidation;

namespace RentalManager.DTOs.Validators.Rules
{
    public static class ContactRules
    {
        public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }

        public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage("Invalid phone number format");
        }
    }
}
