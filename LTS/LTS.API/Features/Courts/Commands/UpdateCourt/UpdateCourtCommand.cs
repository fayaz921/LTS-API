using MediatR;

namespace LTS.API.Features.Courts.Commands.UpdateCourt;

public record UpdateCourtCommand(
    Guid CourtId,
    string CourtName,
    string? AddressContact,
    bool IsActive
) : IRequest<bool>;