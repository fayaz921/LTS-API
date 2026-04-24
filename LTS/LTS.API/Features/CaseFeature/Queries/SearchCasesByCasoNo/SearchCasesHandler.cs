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
                var searchedItem = "";
                var query = _context.Cases.Where(c => c.OrganizationId == request.OrganizationId).AsQueryable();

                if (!string.IsNullOrEmpty(request.CaseNo))
                {
                    query = query.Where(c => c.CaseNo.Contains(request.CaseNo));
                    searchedItem = request.CaseNo;
                }
                else if (!string.IsNullOrEmpty(request.petititionerName))
                {
                    query = query.Where(c => c.CasePetitioners.Any(cp => EF.Functions.ILike(cp.Petitioner.Name, $"%{request.petititionerName}%")));
                    searchedItem = request.petititionerName;
                }
                else if (!string.IsNullOrEmpty(request.Cnic))
                {
                    query = query.Where(c => c.CasePetitioners.Any(cp => EF.Functions.ILike(cp.Petitioner.CNIC ?? string.Empty, $"%{request.Cnic}%")));
                    searchedItem = request.Cnic;
                }

                var data = await query.Select(c => new GetCaseDto(
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
                )).ToListAsync(ct);
                if (!string.IsNullOrEmpty(searchedItem))
                    return data != null ? ApiResponse<List<GetCaseDto>>.Ok(data) : ApiResponse<List<GetCaseDto>>.Fail($"No Result Found with this search '{searchedItem}' !");
                else
                    return data != null ? ApiResponse<List<GetCaseDto>>.Ok(data) : ApiResponse<List<GetCaseDto>>.Fail($"Case List is Empty Currently !");
            }
            catch
            {
                return ApiResponse<List<GetCaseDto>>.Fail("Internal Server error !", HttpStatusCode.InternalServerError);
            }
        }

    }
}
