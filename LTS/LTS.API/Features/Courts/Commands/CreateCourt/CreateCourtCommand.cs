using MediatR;
using LTS.API.Common.Response;

namespace LTS.API.Features.Courts.Commands.CreateCourt
{
    public record CreateCourtCommand(string CourtName, string? AddressContact) : IRequest<ApiResponse<Guid>>;
}