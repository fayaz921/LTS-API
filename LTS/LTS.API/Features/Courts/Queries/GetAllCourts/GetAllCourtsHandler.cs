using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Queries.GetAllCourts;

public class GetAllCourtsHandler : IRequestHandler<GetAllCourtsQuery, List<GetAllCourtsDto>>
{
    private readonly AppDbContext _context;

    public GetAllCourtsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetAllCourtsDto>> Handle(GetAllCourtsQuery request, CancellationToken ct)
    {
        return await _context.Courts
            .Where(x => x.IsActive)
            .Select(x => new GetAllCourtsDto
            {
                Id = x.Id,
                CourtName = x.CourtName,
                AddressContact = x.AddressContact,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
    }
}