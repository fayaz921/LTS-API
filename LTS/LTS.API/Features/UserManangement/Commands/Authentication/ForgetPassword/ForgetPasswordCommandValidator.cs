using FluentValidation;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword
{
    public class ForgetPasswordCommandValidator:AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
