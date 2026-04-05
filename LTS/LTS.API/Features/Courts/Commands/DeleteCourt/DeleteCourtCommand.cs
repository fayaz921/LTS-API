using MediatR;

namespace LTS.API.Features.Courts.Commands.DeleteCourt;

public record DeleteCourtCommand(Guid CourtId) : IRequest<bool>;