using FluentValidation;

namespace LTS.API.Features.Followups.Commands.CreateFollowup
{
    public class CreateFollowupValidator:AbstractValidator<CreateFollowupCommand>
    {
        public CreateFollowupValidator()
        {
            RuleFor(x => x.CaseId)
                .NotEmpty()
                .WithMessage("CaseId is required.");

            RuleFor(x => x.HearingDate)
                .NotEmpty()
                .WithMessage("HearingDate is required.");

            RuleFor(x => x.NextHearingDate)
                .GreaterThan(x => x.HearingDate)
                .When(x => x.NextHearingDate.HasValue)
                .WithMessage("NextHearingDate must be after HearingDate.");
        }
    }
}
