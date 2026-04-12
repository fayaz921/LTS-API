using MediatR;
using LTS.API.Common.Response;

namespace LTS.API.Features.Courts.Commands.UpdateCourt
{
    public record UpdateCourtCommand(
        Guid Id,
        string CourtName,
        string? AddressContact,
        bool IsActive
    ) : IRequest<ApiResponse<string>>;
}