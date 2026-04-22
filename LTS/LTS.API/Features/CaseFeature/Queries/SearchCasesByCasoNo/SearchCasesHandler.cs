using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Features.CaseFeature.Queries.SearchCases;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.SearchCasesByCasoNo
{
    public class SearchCasesHandler(AppDbContext dbContext)
    {
        private readonly AppDbContext _context = dbContext;
        public async Task<ApiResponse<List<GetCaseDto>>> Handle(SearchCasesQuery request, CancellationToken ct)
        {
            var query = _context.Cases.Where(c => c.OrganizationId == request.OrganizationId).AsQueryable();

            if (!string.IsNullOrEmpty(request.CaseNo))
                query = query.Where(c => c.CaseNo.Contains(request.CaseNo));
            if (!string.IsNullOrEmpty(request.petititionerName))
                query = query.Where(c => c.CasePetitioners.Any(cp => EF.Functions.ILike(cp.Petitioner.Name, $"%{request.petititionerName}%")));
            //if (!string.IsNullOrEmpty(request.Cnic))
            //    query = query.Where(c => c.CasePetitioners.Any(cp => EF.Functions.ILike(cp.Petitioner.cnic, $"%{request.Cnic}%")));

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


            return data != null ? ApiResponse<List<GetCaseDto>>.Ok(data) : ApiResponse<List<GetCaseDto>>.Fail("Case List is Empty !");
        }
    }
}
