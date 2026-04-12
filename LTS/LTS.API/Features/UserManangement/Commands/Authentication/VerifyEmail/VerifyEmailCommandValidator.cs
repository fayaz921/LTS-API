using FluentValidation;

namespace LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail
{
    public class VerifyEmailCommandValidator:AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email required hai")
                .EmailAddress().WithMessage("Valid email address do");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Naya password required hai")
                .MinimumLength(8).WithMessage("Password kam se kam 8 characters ka hona chahiye");
        }
    }
}
