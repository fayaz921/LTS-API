using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Features.CaseFeature.Queries.SearchCases;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.SearchCasesByCasoNo
{
    //public class SearchCasesHandler(AppDbContext dbContext) : IRequestHandler<SearchCasesQuery, ApiResponse<List<GetCaseDto>>>
    //{
    //    private readonly AppDbContext _context = dbContext;
    //    public async Task<ApiResponse<List<GetCaseDto>>> Handle(SearchCasesQuery request, CancellationToken ct)
    //    {
    //        try
    //        {
    //            var query = _context.Cases
    //                .Where(c => c.OrganizationId == request.OrganizationId)
    //                .AsNoTracking();

    //            if (!string.IsNullOrWhiteSpace(request.CaseNo))
    //                query = query.Where(c => c.CaseNo.Contains(request.CaseNo));

    //            else if (!string.IsNullOrWhiteSpace(request.PetititionerName))
    //                query = query.Where(c => c.CasePetitioners
    //                    .Any(cp => EF.Functions.ILike(cp.Petitioner.Name, $"{request.PetititionerName}%")));

    //            else if (!string.IsNullOrWhiteSpace(request.Cnic))
    //                query = query.Where(c => c.CasePetitioners
    //                    .Any(cp => EF.Functions.ILike(cp.Petitioner.CNIC ?? string.Empty, $"{request.Cnic}%")));

    //            var data = await query
    //                .OrderByDescending(c => c.CreatedAt)
    //                .Skip((request.Page - 1) * request.PageSize)
    //                .Take(request.PageSize)
    //                .Select(c => new GetCaseDto(
    //                    c.Id,
    //                    c.CaseNo,
    //                    c.Title,
    //                    c.Subject,
    //                    c.DAG,
    //                    c.Status.ToString(),
    //                    c.DateInstitution,
    //                    c.Court != null ? c.Court.CourtName : "N/A",
    //                    c.Department != null ? c.Department.DepartmentName : "N/A",
    //                    c.CasePetitioners.Select(cp => cp.Petitioner.Name).ToList()
    //                ))
    //                .ToListAsync(ct);

    //            return data.Count > 0
    //                ? ApiResponse<List<GetCaseDto>>.Ok(data)
    //                : ApiResponse<List<GetCaseDto>>.Ok("No cases found.");
    //        }
    //        catch (Exception ex)
    //        {
    //            return ApiResponse<List<GetCaseDto>>.Fail($"Internal server error : {ex.Message}", HttpStatusCode.InternalServerError);
    //        }
    //    }

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
                    var query = _context.Cases.AsQueryable();

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
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Include(c => c.Court)
                        .Include(c => c.Department)
                        .Include(c => c.CasePetitioners)
                            .ThenInclude(cp => cp.Petitioner)
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

