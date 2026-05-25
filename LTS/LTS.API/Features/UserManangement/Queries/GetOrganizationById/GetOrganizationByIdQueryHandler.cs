using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetOrganizationById
{
    public sealed class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, ApiResponse<OrganizationDto>>
    {
        private readonly AppDbContext _context;

        public GetOrganizationByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<OrganizationDto>> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .AsNoTracking()
                .Include(o => o.Users)
                .Include(o => o.PaymentRequests)
                .FirstOrDefaultAsync(o => o.Id == request.OrganizationId, cancellationToken);

            if (org is null)
                return ApiResponse<OrganizationDto>.NotFound("Organization not found");

            var petitionerCount = await _context.Petitioners
                .CountAsync(p => p.OrganizationId == org.Id, cancellationToken);

            var caseCount = await _context.Cases
                .CountAsync(c => c.OrganizationId == org.Id, cancellationToken);

            var petitionerCounts = new Dictionary<Guid, int> { [org.Id] = petitionerCount };
            var caseCounts = new Dictionary<Guid, int> { [org.Id] = caseCount };

            var dto = org.ToDto(petitionerCounts, caseCounts);

            return ApiResponse<OrganizationDto>.Ok(dto, "Organization fetched successfully");
        }
    }
}
