using FluentValidation;

using Location.Application.Locations.Query.GetLocationByPostcode;

namespace Location.Application.Locations.Validators;

public class GetLocationByPostcodeQueryValidator : AbstractValidator<GetLocationByPostcodeQuery>
{
    public GetLocationByPostcodeQueryValidator()
    {
        RuleFor(x => x.PostCode)
            .NotEmpty()
            .WithMessage("Postcode is required")
            .MinimumLength(5)
            .WithMessage("Postcode must be at least 5 characters")
            .MaximumLength(8)
            .WithMessage("Postcode must be between 5 and 8 characters");
    }
}