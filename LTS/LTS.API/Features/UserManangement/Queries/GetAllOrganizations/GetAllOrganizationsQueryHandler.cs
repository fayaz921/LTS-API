using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetAllOrganizations
{
    public sealed class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, ApiResponse<PaginatedResponse<OrganizationDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllOrganizationsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedResponse<OrganizationDto>>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Organizations.AsNoTracking().AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(o =>
                    o.OrganizationName.ToLower().Contains(search) ||
                    o.Slug.ToLower().Contains(search));
            }

            if (request.Plan.HasValue)
                query = query.Where(o => o.Plan == request.Plan.Value);

            if (request.IsActive.HasValue)
                query = query.Where(o => o.IsActive == request.IsActive.Value);

            if (request.IsBlocked.HasValue)
                query = query.Where(o => o.IsBlocked == request.IsBlocked.Value);

            if (request.IsSubscriptionActive.HasValue)
                query = query.Where(o => o.IsSubscriptionActive == request.IsSubscriptionActive.Value);

            if (request.IsTrialActive.HasValue)
                query = query.Where(o => o.IsTrialActive == request.IsTrialActive.Value);

            // Count
            var totalCount = await query.CountAsync(cancellationToken);

            var orgIds = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => o.Id)
                .ToListAsync(cancellationToken);

            if (orgIds.Count == 0)
            {
                return ApiResponse<PaginatedResponse<OrganizationDto>>.Ok(
                    PaginatedResponse<OrganizationDto>.Create(
                        [], totalCount, request.PageNumber, request.PageSize),
                    "No organizations found");
            }

            var orgs = await _context.Organizations
                .AsNoTracking()
                .Where(o => orgIds.Contains(o.Id))
                .Include(o => o.Users)
                .Include(o => o.PaymentRequests)
                .ToListAsync(cancellationToken);

            var petitionerCounts = await _context.Petitioners
                .Where(p => orgIds.Contains(p.OrganizationId))
                .GroupBy(p => p.OrganizationId)
                .Select(g => new { OrgId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.OrgId, x => x.Count, cancellationToken);

            var caseCounts = await _context.Cases
                .Where(c => orgIds.Contains(c.OrganizationId))
                .GroupBy(c => c.OrganizationId)
                .Select(g => new { OrgId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.OrgId, x => x.Count, cancellationToken);

            var items = orgIds
                .Select(id => orgs.First(o => o.Id == id))
                .Select(o => o.ToDto(petitionerCounts, caseCounts))
                .ToList();

            var paginated = PaginatedResponse<OrganizationDto>.Create(
                items, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PaginatedResponse<OrganizationDto>>.Ok(
                paginated, "Organizations fetched successfully");
        }
    }
}
