using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;

namespace LTS.API.Features.CaseFeature.Commands.CreateCase
{
    public class CreateCaseHandler(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<ApiResponse<string>> Handle(CreateCaseCommand request,CancellationToken ct)
        {
            await _context.Cases.AddAsync(request.Map(), ct);
            return await _context.SaveChangesAsync(ct) > 0 ? ApiResponse<string>.Created(default!) :
                                                           ApiResponse<string>.Fail("Internel Server Error!", System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
