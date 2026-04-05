using FluentValidation;

namespace LTS.API.Features.Courts.Commands.CreateCourt;

public class CreateCourtValidator : AbstractValidator<CreateCourtCommand>
{
    public CreateCourtValidator()
    {
        RuleFor(x => x.CourtName)
            .NotEmpty().WithMessage("Court name is required")
            .MaximumLength(100).WithMessage("Max 100 characters allowed");

        RuleFor(x => x.AddressContact)
            .MaximumLength(500);
    }
}