using FluentValidation;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty().WithMessage("Organization name is required")
                .MinimumLength(3).WithMessage("Organization name must be at least 3 characters")
                .MaximumLength(100).WithMessage("Organization name must not exceed 100 characters");

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Owner name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number");
        }
    }
}
