using FluentValidation;

namespace LTS.API.Features.Followups.Commands.UpdateFollowup
{
    public class UpdateFollowupValidator:AbstractValidator<UpdateFollowupCommand>
    {
        public UpdateFollowupValidator()
        {
            RuleFor(x => x.FollowupId)
                .NotEmpty()
                .WithMessage("FollowupId is required.");

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
