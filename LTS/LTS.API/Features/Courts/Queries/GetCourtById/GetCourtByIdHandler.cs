using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Features.Courts.DTOs;
using LTS.API.Features.Courts.Mappings;

namespace LTS.API.Features.Courts.Queries.GetCourtById
{
    public class GetCourtByIdHandler : IRequestHandler<GetCourtByIdQuery, ApiResponse<CourtDto>>
    {
        private readonly AppDbContext _context;
        public GetCourtByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CourtDto>> Handle(GetCourtByIdQuery request, CancellationToken ct)
        {
            var court = await _context.Courts
                .Where(x => x.Id == request.Id && x.IsActive)
                .Select(x => x.ToDto())
                .FirstOrDefaultAsync(ct);

            if (court == null)
                return ApiResponse<CourtDto>.NotFound("Court not found");

            return ApiResponse<CourtDto>.Ok(court);
        }
    }
}