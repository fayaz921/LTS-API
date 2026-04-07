using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public class GetCasesHandler(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        public async Task<ApiResponse<List<GetCasesDto>>> Handle(GetAllCasesQuery request, CancellationToken ct)
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
                var caseInfo = cases.ToDto();

                return caseInfo.Any()
                    ? ApiResponse<List<GetCasesDto>>.Ok(caseInfo)
                    : ApiResponse<List<GetCasesDto>>.Ok("Case Table is Empty");
            }
            catch
            {
                return ApiResponse<List<GetCasesDto>>.Fail("Inernel Server error !");
            }
        }
    }
}
