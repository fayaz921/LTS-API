using MediatR;
using LTS.API.Common.Response;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Queries.GetAllCourts
{
    // ─────────────────────────────────────────────────────────────
    // Query — now includes pagination params
    // ─────────────────────────────────────────────────────────────

    public record GetAllCourtsQuery(
        bool? IsActive = null,
        int PageNumber = 1,
        int PageSize = 10
    ) : IRequest<ApiResponse<PaginatedResponse<CourtDto>>>;
}