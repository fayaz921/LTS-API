namespace LTS.API.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> ForgetPasswordOtp(string toEmail, string name, string otp);
        Task<bool> SendRegistrationOtp(string toEmail, string name, string otp);
    }
}
