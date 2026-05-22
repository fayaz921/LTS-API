using LTS.API.Domain.Entities;
using LTS.API.Features.Followups.Commands.CreateFollowup;

namespace LTS.API.Features.Followups.Commands
{
    public static class CommandFollowupMappings
    {
        public static Followup ToEntity(this CreateFollowupCommand command)
        {
            return new Followup
            {
                Id = Guid.NewGuid(),
                CaseId = command.CaseId,
                HearingDate = DateTime.SpecifyKind(command.HearingDate, DateTimeKind.Utc),
                NextHearingDate = command.NextHearingDate.HasValue ? DateTime.SpecifyKind(command.NextHearingDate.Value, DateTimeKind.Utc) : null,
                InterimOrder = command.InterimOrder,
                Decision = command.Decision,
                Remarks = command.Remarks,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"   // Week 3 mein JWT se logged-in user lena
            };
        }
    }
}
