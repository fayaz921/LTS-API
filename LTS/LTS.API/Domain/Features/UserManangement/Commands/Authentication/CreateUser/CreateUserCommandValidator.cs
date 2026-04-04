using FluentValidation;

namespace LTS.API.Domain.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Name is  required ")
        }
    }
}
