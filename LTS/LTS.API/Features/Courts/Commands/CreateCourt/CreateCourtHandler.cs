using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Courts.Commands.CreateCourt;

public class CreateCourtHandler : IRequestHandler<CreateCourtCommand, Guid>
{
    private readonly AppDbContext _context;

    public CreateCourtHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCourtCommand request, CancellationToken ct)
    {
        var court = new Court
        {
            CourtName = request.CourtName,
            AddressContact = request.AddressContact
        };

        _context.Courts.Add(court);
        await _context.SaveChangesAsync(ct);

        return court.Id;
    }
}