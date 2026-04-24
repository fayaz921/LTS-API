using LTS.API.Domain.Entities;

namespace LTS.API.Features.Followups.Queries
{
    public static class QueryFollowupMappings
    {
        public static GetFollowupsByCaseDto ToDto(this Followup followup) => new()
        {
            Id = followup.Id,
            CaseId = followup.CaseId,
            HearingDate = followup.HearingDate,
            NextHearingDate = followup.NextHearingDate,
            InterimOrder = followup.InterimOrder,
            Decision = followup.Decision,
            Remarks = followup.Remarks,
            CreatedAt = followup.CreatedAt,
            CreatedBy = followup.CreatedBy
        };

        public static List<GetFollowupsByCaseDto> ToDto(this List<Followup> followups)
            => followups.Select(f => f.ToDto()).ToList();
    }
}
