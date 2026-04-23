using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Followups.Commands.DeleteFollowup
{
    public record DeleteFollowupCommand(Guid id) : IRequest<ApiResponse<string>>;
   }
