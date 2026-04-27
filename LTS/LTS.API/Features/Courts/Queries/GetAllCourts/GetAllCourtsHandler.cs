using LTS.API.Common.Response;
using LTS.API.Features.Courts.DTOs;
using LTS.API.Features.Courts.Mappings;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Courts.Queries.GetAllCourts;

    public class GetAllCourtsHandler : IRequestHandler<GetAllCourtsQuery, ApiResponse<List<CourtDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllCourtsHandler(AppDbContext context)
        {
            _context = context;
        }

    public async Task<ApiResponse<List<CourtDto>>> Handle(GetAllCourtsQuery request, CancellationToken ct)
    {
        var courts = await _context.Courts
            .Where(x => x.IsActive)
            .Select(x => x.ToDto())
            .ToListAsync(ct);

        return ApiResponse<List<CourtDto>>.Ok(courts);
    }
}