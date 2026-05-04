using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<ApiResponse<ResponseLogin>>;
   
}
