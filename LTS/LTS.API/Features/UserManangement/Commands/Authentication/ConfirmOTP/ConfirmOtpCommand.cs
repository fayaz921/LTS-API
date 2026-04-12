namespace LTS.API.Features.UserManangement.Commands.Authentication.ConfirmOTP
{
    public record ConfirmOtpCommand(string Email, string Otp);
    
}
