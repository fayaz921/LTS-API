using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LTS.API.Features.Followups.Queries.GetFollowupsByCase
{
    public class GetFollowupsByCaseHandler : IRequestHandler<GetFollowupsByCaseQuery, ApiResponse<List<GetFollowupsByCaseDto>>>
    {
        private readonly AppDbContext _context;

        public GetFollowupsByCaseHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<List<GetFollowupsByCaseDto>>> Handle(GetFollowupsByCaseQuery request, CancellationToken cancellationToken)
        {
            var followups = await _context.Followups
                .Where(f => f.CaseId == request.Id)
                .OrderByDescending(f => f.HearingDate)
                .ToListAsync(cancellationToken);

            return ApiResponse<List<GetFollowupsByCaseDto>>.Ok(followups.ToDto());
        }
    }
}
