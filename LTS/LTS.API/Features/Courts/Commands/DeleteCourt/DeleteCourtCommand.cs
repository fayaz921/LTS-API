using MediatR;
using LTS.API.Common.Response;

namespace LTS.API.Features.Courts.Commands.DeleteCourt
{
    public record DeleteCourtCommand(Guid Id) : IRequest<ApiResponse<string>>;
}