using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword
{
    public record ForgetPasswordCommand(string Email) : IRequest<ApiResponse<string>>;
  
}
