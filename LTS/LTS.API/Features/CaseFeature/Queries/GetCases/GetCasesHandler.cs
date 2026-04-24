using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public class GetCasesHandler(AppDbContext context):IRequestHandler<GetAllCasesQuery,ApiResponse<List<GetCaseDto>>>
    {
        private readonly AppDbContext _context = context;
        public async Task<ApiResponse<List<GetCaseDto>>> Handle(GetAllCasesQuery request, CancellationToken ct)
        {
            try
            {
                var cases = await _context.Cases.Include(c => c.Court)
                                                .Include(c => c.Department)
                                                .Include(c => c.CasePetitioners)
                                                    .ThenInclude(cp => cp.Petitioner)
                                                .AsNoTracking()
                                                .OrderByDescending(c => c.CreatedAt)
                                                .ToListAsync(ct);
                var caseInfo = cases.ToGetAllCasesDto();

                return caseInfo.Any()
                    ? ApiResponse<List<GetCaseDto>>.Ok(caseInfo)
                    : ApiResponse<List<GetCaseDto>>.Ok("Case Table is Empty");
            }
            catch
            {
                return ApiResponse<List<GetCaseDto>>.Fail("Inernel Server error !");
            }
        }
    }
}
