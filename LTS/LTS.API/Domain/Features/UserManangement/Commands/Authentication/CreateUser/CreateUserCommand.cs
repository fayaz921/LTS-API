using MediatR;

namespace LTS.API.Domain.Features.UserManangement.Commands.Authentication.CreateUser
{
    public record CreateUserCommand(string FullName,  string Email, string Password):IRequest<string>;

}
