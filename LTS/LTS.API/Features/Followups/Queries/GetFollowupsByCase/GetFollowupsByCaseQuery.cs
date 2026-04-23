using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Followups.Queries.GetFollowupsByCase
{
    public record GetFollowupsByCaseQuery(Guid Id) : IRequest<ApiResponse<List<GetFollowupsByCaseDto>>>;
}
