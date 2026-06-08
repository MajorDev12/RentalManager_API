using FluentValidation;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Validators.Rules;

namespace RentalManager.DTOs.Validators.Property
{
    public class UPDATEPropertyValidator : AbstractValidator<PATCHPropertyDto>
    {
        public UPDATEPropertyValidator()
        {
            RuleFor(x => x.Name)
                .RequiredName()
                .When(x => x.Name != null);

            RuleFor(x => x.EmailAddress)
            .Email()
            .When(x => x.EmailAddress != null);

            RuleFor(x => x.MobileNumber)
                .Phone()
                .When(x => x.MobileNumber != null);

            RuleFor(x => x.PhysicalAddress)
                .RequiredAddress()
                .When(x => x.PhysicalAddress != null);

            RuleFor(x => x.Country)
                .RequiredLocation()
                .When(x => x.Country != null);

            RuleFor(x => x.County)
                .RequiredLocation()
                .When(x => x.County != null);

            RuleFor(x => x.Area)
                .RequiredLocation()
                .When(x => x.Area != null);

            RuleFor(x => x.PropertyTypeId)
                .ValidId()
                .When(x => x.PropertyTypeId > 0);
        }
    }
}
