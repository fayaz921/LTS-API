using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<ApiResponse<string>>;
   
}
