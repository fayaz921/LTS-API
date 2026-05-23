using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.SearchCases
{
    public class SearchCasesHandler : IRequestHandler<SearchCasesQuery, ApiResponse<PagedResult<GetCaseDto>>>
    {
        private readonly AppDbContext _context;

        public SearchCasesHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PagedResult<GetCaseDto>>> Handle(SearchCasesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Start with base query
                var query = _context.Cases
                                        .Where(c => c.OrganizationId == request.OrganizationId)
                                        .AsNoTracking()
                                        .AsQueryable();

                // 1. SEARCH FILTER - CaseNo, Title, ya Petitioner CNIC par search
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower().Trim();
                    query = query.Where(c =>
                        c.CaseNo.ToLower().Contains(searchTerm) ||
                        c.Title.ToLower().Contains(searchTerm) ||
                        c.CasePetitioners.Any(cp => cp.Petitioner.CNIC != null && cp.Petitioner.CNIC.ToLower().Contains(searchTerm))
                    );
                }

                // 2. STATUS FILTER
                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    if (Enum.TryParse<CaseStatus>(request.Status, true, out var statusEnum))
                    {
                        query = query.Where(c => c.Status == statusEnum);
                    }
                }

                // 3. DATE RANGE FILTER
                if (request.DateFrom.HasValue)
                {
                    query = query.Where(c => c.DateInstitution >= request.DateFrom.Value);
                }
                if (request.DateTo.HasValue)
                {
                    var toDateWithEndOfDay = request.DateTo.Value.AddDays(1);
                    query = query.Where(c => c.DateInstitution < toDateWithEndOfDay);
                }

                // 4. SORTING - newest first
                query = query.OrderByDescending(c => c.DateInstitution).ThenByDescending(c => c.CreatedAt);

                // 5. TOTAL COUNT (before pagination)
                var totalCount = await query.CountAsync(cancellationToken);

                // 6. PAGINATION
                var cases = await query
                    .OrderByDescending(c => c.DateInstitution)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new GetCaseDto(
                             Id: c.Id,
                             CaseNo: c.CaseNo,
                             Title: c.Title,
                             Subject: c.Subject,
                             DAG: c.DAG,
                             Status: c.Status.ToString(),
                             DateInstitution: c.DateInstitution,
                             CourtName: c.Court.CourtName ?? "N/A",
                             DepartmentName: c.Department.DepartmentName ?? "N/A",
                             Petitioners: c.CasePetitioners
                                 .Select(cp => new PetitionerDetailDto(
                                     Id: cp.Petitioner.Id,
                                     Name: cp.Petitioner.Name,
                                     CNIC: cp.Petitioner.CNIC,
                                     Email: cp.Petitioner.Email,
                                     Phone: cp.Petitioner.Phone
                                 ))
                                 .ToList()
                         )).ToListAsync(cancellationToken);

                // 8. RETURN PAGED RESPONSE
                var pagedResponse = new PagedResult<GetCaseDto>
                {
                    Items = cases,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                };

                return ApiResponse<PagedResult<GetCaseDto>>.Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetCaseDto>>.Fail($"Search failed: {ex.Message}");
            }
        }
    }

}

