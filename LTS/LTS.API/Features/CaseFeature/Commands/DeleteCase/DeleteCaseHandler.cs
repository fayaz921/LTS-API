using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseFeature.Commands.DeleteCase
{
    public class DeleteCaseHandler
    {
        private readonly AppDbContext _context;

        public DeleteCaseHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCaseCommand request, CancellationToken ct)
        {
            var caseInfo = await _context.Cases.Where(x => x.Id == request.CaseId).FirstOrDefaultAsync(ct);
            if (caseInfo == null)
                return ApiResponse<string>.Fail("Case mot foumd");

            _context.Cases.Remove(caseInfo);
            return await _context.SaveChangesAsync(ct) > 0 ? ApiResponse<string>.Ok(default!, "Case Deleted Succesfuly !") : ApiResponse<string>.Fail("Server Error !", HttpStatusCode.InternalServerError);
        }
    }
}
