using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Followups.Commands.UpdateFollowup
{
    public record UpdateFollowupCommand(
    Guid FollowupId,
    DateTime HearingDate,
    DateTime? NextHearingDate,
    string? InterimOrder,
    string? Decision,
    string? Remarks
) : IRequest<ApiResponse<string>>;

}
