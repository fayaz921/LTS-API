using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ConfirmOTP
{
    public record VerifyOtpCommand(string Email, string Otp) : IRequest<ApiResponse<string>>;

}
