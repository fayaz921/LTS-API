using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;

namespace LTS.API.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> ForgetPasswordOtp(string toEmail, string name, string otp)
        {
            var subject = "Reset Your LTS Password";
            var body = $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                    <title>Reset Your Password</title>
                </head>
                <body style="margin:0; padding:0; background-color:#f4f6f9; font-family: 'Segoe UI', Arial, sans-serif;">
 
                    <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f6f9; padding: 40px 0;">
                        <tr>
                            <td align="center">
 
                                <!-- Main Card -->
                                <table width="600" cellpadding="0" cellspacing="0"
                                    style="background-color:#ffffff; border-radius:16px;
                                           box-shadow: 0 4px 24px rgba(0,0,0,0.08); overflow:hidden;">
 
                                    <!-- Header -->
                                    <tr>
                                        <td style="background: linear-gradient(135deg, #4F46E5 0%, #7C3AED 100%);
                                                   padding: 40px 48px; text-align:center;">
                                            <h1 style="margin:0; color:#ffffff; font-size:28px;
                                                       font-weight:700; letter-spacing:-0.5px;">
                                                LTS
                                            </h1>
                                            <p style="margin:6px 0 0; color:rgba(255,255,255,0.75);
                                                      font-size:13px; letter-spacing:2px; text-transform:uppercase;">
                                                Litigation Tracking System
                                            </p>
                                        </td>
                                    </tr>
 
                                    <!-- Lock Icon Row -->
                                    <tr>
                                        <td align="center" style="padding: 40px 48px 0;">
                                            <div style="width:72px; height:72px; border-radius:50%;
                                                        background:#EEF2FF; display:inline-flex;
                                                        align-items:center; justify-content:center;
                                                        font-size:32px; line-height:72px; text-align:center;">
                                                &#128274;
                                            </div>
                                        </td>
                                    </tr>
 
                                    <!-- Body -->
                                    <tr>
                                        <td style="padding: 28px 48px 0;">
                                            <h2 style="margin:0 0 8px; color:#111827;
                                                       font-size:22px; font-weight:700;">
                                                Password Reset Request
                                            </h2>
                                            <p style="margin:0 0 24px; color:#6B7280; font-size:15px; line-height:1.6;">
                                                Hi <strong style="color:#111827;">{name}</strong>,
                                            </p>
                                            <p style="margin:0 0 24px; color:#6B7280; font-size:15px; line-height:1.6;">
                                                We received a request to reset the password for your LTS account.
                                                Use the OTP below to proceed. This code is valid for
                                                <strong style="color:#111827;">10 minutes</strong> only.
                                            </p>
                                        </td>
                                    </tr>
 
                                    <!-- OTP Box -->
                                    <tr>
                                        <td style="padding: 0 48px;">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center"
                                                        style="background: linear-gradient(135deg, #EEF2FF 0%, #F5F3FF 100%);
                                                               border: 2px dashed #C7D2FE;
                                                               border-radius:12px;
                                                               padding: 28px 20px;">
                                                        <p style="margin:0 0 8px; color:#6B7280;
                                                                  font-size:12px; letter-spacing:3px;
                                                                  text-transform:uppercase; font-weight:600;">
                                                            Your One-Time Password
                                                        </p>
                                                        <p style="margin:0; color:#4F46E5;
                                                                  font-size:42px; font-weight:800;
                                                                  letter-spacing:14px; font-family:monospace;">
                                                            {otp}
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
 
                                    <!-- Expiry Warning -->
                                    <tr>
                                        <td style="padding: 20px 48px 0;">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="background:#FEF3C7; border-left: 4px solid #F59E0B;
                                                               border-radius:0 8px 8px 0; padding:14px 16px;">
                                                        <p style="margin:0; color:#92400E; font-size:13px; line-height:1.5;">
                                                            &#9201;&nbsp;
                                                            <strong>This OTP will expire in 10 minutes.</strong>
                                                            If you did not request a password reset, 
                                                            please ignore this email or contact support immediately.
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
 
                                    <!-- Security Tips -->
                                    <tr>
                                        <td style="padding: 28px 48px 0;">
                                            <table width="100%" cellpadding="0" cellspacing="0"
                                                   style="border:1px solid #E5E7EB; border-radius:10px;">
                                                <tr>
                                                    <td style="padding:18px 20px; border-bottom:1px solid #E5E7EB;">
                                                        <p style="margin:0; color:#374151; font-size:13px;
                                                                  font-weight:600; letter-spacing:1px;
                                                                  text-transform:uppercase;">
                                                            Security Tips
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding:16px 20px;">
                                                        <p style="margin:0 0 10px; color:#6B7280;
                                                                  font-size:13px; line-height:1.6;">
                                                            &#10003;&nbsp; Never share this OTP with anyone
                                                        </p>
                                                        <p style="margin:0 0 10px; color:#6B7280;
                                                                  font-size:13px; line-height:1.6;">
                                                            &#10003;&nbsp; LTS team will never ask for your OTP
                                                        </p>
                                                        <p style="margin:0; color:#6B7280;
                                                                  font-size:13px; line-height:1.6;">
                                                            &#10003;&nbsp; Use a strong password with letters, numbers & symbols
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
 
                                    <!-- Divider -->
                                    <tr>
                                        <td style="padding: 36px 48px 0;">
                                            <hr style="border:none; border-top:1px solid #E5E7EB; margin:0;" />
                                        </td>
                                    </tr>
 
                                    <!-- Footer -->
                                    <tr>
                                        <td style="padding: 24px 48px 40px; text-align:center;">
                                            <p style="margin:0 0 6px; color:#9CA3AF; font-size:12px; line-height:1.6;">
                                                This email was sent to
                                                <span style="color:#4F46E5;">{name}</span>
                                                because a password reset was requested for your LTS account.
                                            </p>
                                            <p style="margin:0; color:#9CA3AF; font-size:12px;">
                                                &copy; {DateTime.UtcNow.Year} Litigation Tracking System. All rights reserved.
                                            </p>
                                        </td>
                                    </tr>
 
                                </table>
                                <!-- End Main Card -->
 
                            </td>
                        </tr>
                    </table>
 
                </body>
                </html>
                """;
            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendRegistrationOtp(string toEmail, string name, string otp)
        {
            var subject = "Verify Your LTS Account";
            var body = $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
    <title>Verify Your Account</title>
</head>
<body style='margin:0; padding:0; background-color:#f4f6f9; font-family: Segoe UI, Arial, sans-serif;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f9; padding: 40px 0;'>
        <tr><td align='center'>
            <table width='600' cellpadding='0' cellspacing='0'
                style='background-color:#ffffff; border-radius:16px; box-shadow: 0 4px 24px rgba(0,0,0,0.08); overflow:hidden;'>

                <!-- Header -->
                <tr>
                    <td style='background: linear-gradient(135deg, #4F46E5 0%, #7C3AED 100%); padding: 40px 48px; text-align:center;'>
                        <h1 style='margin:0; color:#ffffff; font-size:28px; font-weight:700; letter-spacing:-0.5px;'>LTS</h1>
                        <p style='margin:6px 0 0; color:rgba(255,255,255,0.75); font-size:13px; letter-spacing:2px; text-transform:uppercase;'>
                            Litigation Tracking System
                        </p>
                    </td>
                </tr>

                <!-- Icon -->
                <tr>
                    <td align='center' style='padding: 40px 48px 0;'>
                        <div style='width:72px; height:72px; border-radius:50%; background:#EEF2FF;
                                    font-size:32px; line-height:72px; text-align:center;'>&#9989;</div>
                    </td>
                </tr>

                <!-- Body -->
                <tr>
                    <td style='padding: 28px 48px 0;'>
                        <h2 style='margin:0 0 8px; color:#111827; font-size:22px; font-weight:700;'>
                            Verify Your Email Address
                        </h2>
                        <p style='margin:0 0 24px; color:#6B7280; font-size:15px; line-height:1.6;'>
                            Hi <strong style='color:#111827;'>{name}</strong>,
                        </p>
                        <p style='margin:0 0 24px; color:#6B7280; font-size:15px; line-height:1.6;'>
                            Welcome to <strong style='color:#111827;'>LTS</strong>! Your account has been created successfully.
                            Please use the OTP below to verify your email and activate your account.
                            This code is valid for <strong style='color:#111827;'>10 minutes</strong> only.
                        </p>
                    </td>
                </tr>

                <!-- OTP Box -->
                <tr>
                    <td style='padding: 0 48px;'>
                        <table width='100%' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td align='center'
                                    style='background: linear-gradient(135deg, #EEF2FF 0%, #F5F3FF 100%);
                                           border: 2px dashed #C7D2FE; border-radius:12px; padding: 28px 20px;'>
                                    <p style='margin:0 0 8px; color:#6B7280; font-size:12px;
                                              letter-spacing:3px; text-transform:uppercase; font-weight:600;'>
                                        Your One-Time Password
                                    </p>
                                    <p style='margin:0; color:#4F46E5; font-size:42px; font-weight:800;
                                              letter-spacing:14px; font-family:monospace;'>
                                        {otp}
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <!-- Expiry Warning -->
                <tr>
                    <td style='padding: 20px 48px 0;'>
                        <table width='100%' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td style='background:#FEF3C7; border-left: 4px solid #F59E0B;
                                           border-radius:0 8px 8px 0; padding:14px 16px;'>
                                    <p style='margin:0; color:#92400E; font-size:13px; line-height:1.5;'>
                                        &#9201;&nbsp;
                                        <strong>This OTP will expire in 10 minutes.</strong>
                                        If you did not create an LTS account, please ignore this email.
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <!-- What happens next -->
                <tr>
                    <td style='padding: 28px 48px 0;'>
                        <table width='100%' cellpadding='0' cellspacing='0'
                               style='border:1px solid #E5E7EB; border-radius:10px;'>
                            <tr>
                                <td style='padding:18px 20px; border-bottom:1px solid #E5E7EB;'>
                                    <p style='margin:0; color:#374151; font-size:13px;
                                              font-weight:600; letter-spacing:1px; text-transform:uppercase;'>
                                        What happens after verification?
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding:16px 20px;'>
                                    <p style='margin:0 0 10px; color:#6B7280; font-size:13px; line-height:1.6;'>
                                        &#127381;&nbsp; Your <strong style='color:#111827;'>7-day free trial</strong> will start immediately
                                    </p>
                                    <p style='margin:0 0 10px; color:#6B7280; font-size:13px; line-height:1.6;'>
                                        &#128100;&nbsp; You can add up to <strong style='color:#111827;'>2 users</strong> during the trial
                                    </p>
                                    <p style='margin:0; color:#6B7280; font-size:13px; line-height:1.6;'>
                                        &#128193;&nbsp; You can add up to <strong style='color:#111827;'>5 clients</strong> during the trial
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <!-- Divider -->
                <tr>
                    <td style='padding: 36px 48px 0;'>
                        <hr style='border:none; border-top:1px solid #E5E7EB; margin:0;' />
                    </td>
                </tr>

                <!-- Footer -->
                <tr>
                    <td style='padding: 24px 48px 40px; text-align:center;'>
                        <p style='margin:0 0 6px; color:#9CA3AF; font-size:12px; line-height:1.6;'>
                            This email was sent to <span style='color:#4F46E5;'>{toEmail}</span>
                            because an account was created using this address.
                        </p>
                        <p style='margin:0; color:#9CA3AF; font-size:12px;'>
                            &copy; {DateTime.UtcNow.Year} Litigation Tracking System. All rights reserved.
                        </p>
                    </td>
                </tr>

            </table>
        </td></tr>
    </table>
</body>
</html>";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send email");
                return false;
            }
        }
    }
}
