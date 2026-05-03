using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Features.CaseFeature.Queries.SearchCases;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseFeature.Queries.SearchCasesByCasoNo
{
    public class SearchCasesHandler(AppDbContext dbContext) : IRequestHandler<SearchCasesQuery, ApiResponse<List<GetCaseDto>>>
    {
        private readonly AppDbContext _context = dbContext;
        public async Task<ApiResponse<List<GetCaseDto>>> Handle(SearchCasesQuery request, CancellationToken ct)
        {
            try
            {
                var query = _context.Cases
                    .Where(c => c.OrganizationId == request.OrganizationId)
                    .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.CaseNo))
                    query = query.Where(c => c.CaseNo.Contains(request.CaseNo));

                else if (!string.IsNullOrWhiteSpace(request.PetititionerName))
                    query = query.Where(c => c.CasePetitioners
                        .Any(cp => EF.Functions.ILike(cp.Petitioner.Name, $"{request.PetititionerName}%")));

                else if (!string.IsNullOrWhiteSpace(request.Cnic))
                    query = query.Where(c => c.CasePetitioners
                        .Any(cp => EF.Functions.ILike(cp.Petitioner.CNIC ?? string.Empty, $"{request.Cnic}%")));

                var data = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new GetCaseDto(
                        c.Id,
                        c.CaseNo,
                        c.Title,
                        c.Subject,
                        c.DAG,
                        c.Status.ToString(),
                        c.DateInstitution,
                        c.Court != null ? c.Court.CourtName : "N/A",
                        c.Department != null ? c.Department.DepartmentName : "N/A",
                        c.CasePetitioners.Select(cp => cp.Petitioner.Name).ToList()
                    ))
                    .ToListAsync(ct);

                return data.Count > 0
                    ? ApiResponse<List<GetCaseDto>>.Ok(data)
                    : ApiResponse<List<GetCaseDto>>.Ok("No cases found.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetCaseDto>>.Fail($"Internal server error : {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

    }
}
