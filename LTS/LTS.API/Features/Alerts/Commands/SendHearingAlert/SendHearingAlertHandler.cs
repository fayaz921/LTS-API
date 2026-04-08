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
            var body = $@"
                <h2>Upcoming Hearing Reminder</h2>
                <p><strong>Case No:</strong> {caseEntity.CaseNo}</p>
                <p><strong>Title:</strong> {caseEntity.Title}</p>
                <p><strong>Next Hearing Date:</strong> {nextFollowup!.NextHearingDate:dd MMM yyyy}</p>
                <p>Please be prepared for the upcoming hearing.</p>
            ";

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
    }
}
