using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Courts.Commands.DeleteCourt;

public class DeleteCourtHandler : IRequestHandler<DeleteCourtCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteCourtHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCourtCommand request, CancellationToken ct)
    {
        var court = await _context.Courts.FindAsync(request.CourtId);

        if (court == null)
            return false;

        // Soft delete
        court.IsActive = false;

        await _context.SaveChangesAsync(ct);

        return true;
    }
}