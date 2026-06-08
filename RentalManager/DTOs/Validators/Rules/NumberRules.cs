using FluentValidation;

namespace RentalManager.DTOs.Validators.Rules
{
    public static class NumberRules
    {
        // ===== NON-NULLABLE INT =====
        public static IRuleBuilderOptions<T, int> ValidId<T>(
            this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be a valid positive number.");
        }

        // ===== NULLABLE INT =====
        public static IRuleBuilderOptions<T, int?> ValidId<T>(
            this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be a valid positive number.");
        }

        public static IRuleBuilderOptions<T, decimal> ValidAmount<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(1_000_000)
                .WithMessage("{PropertyName} is too large")
                .PrecisionScale(18, 2, true)
                .WithMessage("{PropertyName} cannot exceed 2 decimal places");
        }

        public static IRuleBuilderOptions<T, decimal?> ValidAmount<T>(
            this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(1_000_000)
                .WithMessage("{PropertyName} is too large")
                .PrecisionScale(18, 2, true)
                .WithMessage("{PropertyName} cannot exceed 2 decimal places");
        }
    }
}