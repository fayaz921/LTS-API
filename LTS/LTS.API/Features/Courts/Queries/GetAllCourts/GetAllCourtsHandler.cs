using LTS.API.Common.Response;
using LTS.API.Features.Courts.DTOs;
using LTS.API.Features.Courts.Mappings;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Courts.Queries.GetAllCourts
{
    // ─────────────────────────────────────────────────────────────
    // Handler — Skip/Take pagination, TotalCount from DB
    // ─────────────────────────────────────────────────────────────

    public class GetAllCourtsHandler
        : IRequestHandler<GetAllCourtsQuery, ApiResponse<PaginatedResponse<CourtDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllCourtsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedResponse<CourtDto>>> Handle(
            GetAllCourtsQuery   request,
            CancellationToken   ct)
        {
            // ── Validate page params ──────────────────────────────
            var pageNumber = request.PageNumber < 1 ? 1  : request.PageNumber;
            var pageSize   = request.PageSize   < 1 ? 10 : request.PageSize > 100 ? 100 : request.PageSize;

            // ── Base query ────────────────────────────────────────
            var query = _context.Courts.AsQueryable();

            if (request.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.IsActive.Value);

            // ── Total count BEFORE pagination ─────────────────────
            var totalCount = await query.CountAsync(ct);

            // ── Apply pagination ──────────────────────────────────
            var items = await query
                .OrderByDescending(x => x.CreatedAt)          // newest first
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.ToDto())
                .ToListAsync(ct);

            // ── Build paginated response ──────────────────────────
            var paginated = PaginatedResponse<CourtDto>.Create(
                items,
                totalCount,
                pageNumber,
                pageSize);

            return ApiResponse<PaginatedResponse<CourtDto>>.Ok(paginated);
        }
    }
}