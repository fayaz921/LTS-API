using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseFeature.Commands.UpdateCase
{
    public class UpdateCaseHandler(AppDbContext context) : IRequestHandler<UpdateCaseCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context = context;

        public async Task<ApiResponse<string>> Handle(UpdateCaseCommand request, CancellationToken ct)
        {
            var caseInfo = await _context.Cases.Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
            if (caseInfo == null)
                return ApiResponse<string>.Fail("Case mot foumd");

            _context.Cases.Update(request.MapToUpdatedCase(caseInfo));
            return await _context.SaveChangesAsync(ct) > 0 ? ApiResponse<string>.Ok(default!, "Case Data Updated Succesfuly !") : ApiResponse<string>.Fail("Server Error !", HttpStatusCode.InternalServerError);
        }
    }
}
