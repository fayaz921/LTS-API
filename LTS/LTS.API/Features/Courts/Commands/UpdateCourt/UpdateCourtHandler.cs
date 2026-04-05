using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Courts.Commands.UpdateCourt;

public class UpdateCourtHandler : IRequestHandler<UpdateCourtCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateCourtHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCourtCommand request, CancellationToken ct)
    {
        var court = await _context.Courts.FindAsync(request.CourtId);

        if (court == null)
            return false;

        court.CourtName = request.CourtName;
        court.AddressContact = request.AddressContact;
        court.IsActive = request.IsActive;

        await _context.SaveChangesAsync(ct);

        return true;
    }
}