using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseFeature.Queries.GetById
{
    public class GetCaseQueryHandler(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        public async Task<ApiResponse<GetCaseDto>> Handle(GetCaseByIdQuery request, CancellationToken ct)
        {
            try
            {
                var caseInfo = await _context.Cases.Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
                return caseInfo?.ToGetCaseDto() != null
                    ? ApiResponse<GetCaseDto>.Ok(caseInfo.ToGetCaseDto())
                    : ApiResponse<GetCaseDto>.Ok("Case Not Found", HttpStatusCode.NotFound);

            }
            catch
            {
                return ApiResponse<GetCaseDto>.Fail("Server Error !", HttpStatusCode.InternalServerError);
            }
        }
    }
}