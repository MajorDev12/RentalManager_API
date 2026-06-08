using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using RentalManager.DTOs.Validators.Property;

namespace RentalManager.DI.Validation
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddValidationServices(
            this IServiceCollection services)
        {
            // Register all validators in the assembly
            services.AddValidatorsFromAssemblyContaining<CREATEPropertyValidator>();
            services.AddValidatorsFromAssemblyContaining<UPDATEPropertyValidator>();

            // Enable automatic validation
            services.AddFluentValidationAutoValidation();

            // Custom API response for validation errors
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var response = ApiResponse<object>.FailResponse(
                        "Validation failed",
                        errors
                    );

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}