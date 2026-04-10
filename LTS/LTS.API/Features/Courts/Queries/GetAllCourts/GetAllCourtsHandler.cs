using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Features.Courts.DTOs;

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
                .Select(x => new CourtDto
                {
                    Id = x.Id,
                    CourtName = x.CourtName,
                    AddressContact = x.AddressContact,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);

            return ApiResponse<List<CourtDto>>.Ok(courts);
        }
    }