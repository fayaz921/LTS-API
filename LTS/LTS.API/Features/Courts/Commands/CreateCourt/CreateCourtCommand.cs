using MediatR;

namespace LTS.API.Features.Courts.Commands.CreateCourt;

public record CreateCourtCommand(string CourtName, string? AddressContact) : IRequest<Guid>;
