using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail
{
    public record VerifyEmailCommand(string Email, string Otp, string NewPassword) : IRequest<ApiResponse<string>>;
  
}
