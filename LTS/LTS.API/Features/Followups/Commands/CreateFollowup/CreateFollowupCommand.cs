using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Followups.Commands.CreateFollowup
{
    public record CreateFollowupCommand(
    Guid CaseId,
    DateTime HearingDate,
    DateTime? NextHearingDate,
    string? InterimOrder,
    string? Decision,
    string? Remarks
        ) : IRequest<ApiResponse<string>>;
  
}
