using FluentValidation;

namespace LTS.API.Features.Courts.Commands.UpdateCourt
{
    public class UpdateCourtValidator : AbstractValidator<UpdateCourtCommand>
    {
        public UpdateCourtValidator()
        {
            RuleFor(x => x.CourtName)
                .NotEmpty().WithMessage("Court name is required")
                .MaximumLength(100);

            RuleFor(x => x.AddressContact)
                .MaximumLength(500);
        }
    }
}