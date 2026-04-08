using FluentValidation;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("Name is  required ");
        }
    }
}
