using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Alerts.Commands.SendHearingAlert
{
    public class SendHearingAlertHandler : IRequestHandler<SendHearingAlertCommand, ApiResponse<bool>>
    {
        private readonly IEmailService emailService;
        private readonly AppDbContext context;

        public SendHearingAlertHandler(IEmailService emailService,AppDbContext context)
        {
            this.emailService = emailService;
            this.context = context;
        }
        public async Task<ApiResponse<bool>> Handle(SendHearingAlertCommand request, CancellationToken cancellationToken)
        {
            var caseEntity = await context.Cases.Include(c => c.Followups).FirstOrDefaultAsync(c => c.Id == request.CaseID, cancellationToken);
            if (caseEntity == null)
                return ApiResponse<bool>.Fail("Case not found.", System.Net.HttpStatusCode.NotFound);
            var nextFollowup = caseEntity!.Followups
              .Where(f => f.NextHearingDate.HasValue)
              .OrderBy(f => f.NextHearingDate)
              .FirstOrDefault();
            if (nextFollowup == null || !nextFollowup.NextHearingDate.HasValue)
               return  ApiResponse<bool>.Fail("No upcoming hearing found for this case.", System.Net.HttpStatusCode.NotFound);
            if (string.IsNullOrWhiteSpace(caseEntity.EmailList))
                return ApiResponse<bool>.Fail("No email addresses found for this case.", System.Net.HttpStatusCode.BadRequest);
            var subject = $"Upcoming Hearing Alert - {caseEntity.CaseNo}";
            var body = BuildHearingAlertBody(caseEntity.CaseNo, caseEntity.Title, nextFollowup.NextHearingDate!.Value);
            var emails = caseEntity.EmailList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            foreach (var email in emails)
            {
                await emailService.SendEmailAsync(email, subject, body);
            }

            return ApiResponse<bool>.Ok(true, "Hearing alert sent successfully.");
        }
        private static string BuildHearingAlertBody(string caseNo, string title, DateTime hearingDate) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
            <title>Upcoming Hearing Reminder</title>
        </head>
        <body style="margin:0; padding:0; background-color:#f4f6f9; font-family: 'Segoe UI', Arial, sans-serif;">
            <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f6f9; padding: 40px 0;">
                <tr><td align="center">
                    <table width="600" cellpadding="0" cellspacing="0"
                        style="background-color:#ffffff; border-radius:16px; box-shadow: 0 4px 24px rgba(0,0,0,0.08); overflow:hidden;">

                        <!-- Header -->
                        <tr>
                            <td style="background: linear-gradient(135deg, #4F46E5 0%, #7C3AED 100%); padding: 40px 48px; text-align:center;">
                                <h1 style="margin:0; color:#ffffff; font-size:28px; font-weight:700; letter-spacing:-0.5px;">LTS</h1>
                                <p style="margin:6px 0 0; color:rgba(255,255,255,0.75); font-size:13px; letter-spacing:2px; text-transform:uppercase;">
                                    Litigation Tracking System
                                </p>
                            </td>
                        </tr>

                        <!-- Icon -->
                        <tr>
                            <td align="center" style="padding: 40px 48px 0;">
                                <div style="width:72px; height:72px; border-radius:50%; background:#EEF2FF; font-size:32px; line-height:72px; text-align:center;">
                                    &#9878;
                                </div>
                            </td>
                        </tr>

                        <!-- Body -->
                        <tr>
                            <td style="padding: 28px 48px 0;">
                                <h2 style="margin:0 0 8px; color:#111827; font-size:22px; font-weight:700;">Upcoming Hearing Reminder</h2>
                                <p style="margin:0 0 24px; color:#6B7280; font-size:15px; line-height:1.6;">
                                    You have an upcoming court hearing scheduled. Please review the details below.
                                </p>
                            </td>
                        </tr>

                        <!-- Case Details -->
                        <tr>
                            <td style="padding: 0 48px;">
                                <table width="100%" cellpadding="0" cellspacing="0"
                                    style="background: linear-gradient(135deg, #EEF2FF 0%, #F5F3FF 100%); border: 2px solid #C7D2FE; border-radius:12px;">
                                    <tr>
                                        <td style="padding: 20px 24px; border-bottom: 1px solid #C7D2FE;">
                                            <p style="margin:0; color:#374151; font-size:13px; font-weight:600; letter-spacing:1px; text-transform:uppercase;">Case Details</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 20px 24px;">
                                            <p style="margin:0; color:#6B7280; font-size:13px;">Case No</p>
                                            <p style="margin:4px 0 14px; color:#111827; font-size:15px; font-weight:600;">{caseNo}</p>

                                            <p style="margin:0; color:#6B7280; font-size:13px;">Case Title</p>
                                            <p style="margin:4px 0 14px; color:#111827; font-size:15px; font-weight:600;">{title}</p>

                                            <p style="margin:0; color:#6B7280; font-size:13px;">Next Hearing Date</p>
                                            <p style="margin:4px 0 0; color:#4F46E5; font-size:20px; font-weight:800; font-family:monospace; letter-spacing:2px;">
                                                {hearingDate:dd MMM yyyy}
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <!-- Warning -->
                        <tr>
                            <td style="padding: 20px 48px 0;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="background:#FEF3C7; border-left: 4px solid #F59E0B; border-radius:0 8px 8px 0; padding:14px 16px;">
                                            <p style="margin:0; color:#92400E; font-size:13px; line-height:1.5;">
                                                &#9201;&nbsp;
                                                <strong>Ensure all case documents are ready before the hearing date.</strong>
                                                Contact the assigned attorney if you need assistance.
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <!-- Divider -->
                        <tr><td style="padding: 36px 48px 0;"><hr style="border:none; border-top:1px solid #E5E7EB; margin:0;" /></td></tr>

                        <!-- Footer -->
                        <tr>
                            <td style="padding: 24px 48px 40px; text-align:center;">
                                <p style="margin:0 0 6px; color:#9CA3AF; font-size:12px; line-height:1.6;">
                                    This is an automated reminder from the Litigation Tracking System.
                                </p>
                                <p style="margin:0; color:#9CA3AF; font-size:12px;">
                                    &copy; {DateTime.UtcNow.Year} Litigation Tracking System. All rights reserved.
                                </p>
                            </td>
                        </tr>

                    </table>
                </td></tr>
            </table>
        </body>
        </html>
        """;
    }
}
