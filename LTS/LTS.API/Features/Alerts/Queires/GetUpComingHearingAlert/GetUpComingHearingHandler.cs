using LTS.API.Common.Response;
using LTS.API.Features.Alerts.DTOs;
using LTS.API.Features.Alerts.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Alerts.Queires.GetUpComingHearingAlert
{
    public class GetUpComingHearingHandler : IRequestHandler<GetUpComingHearingQuery, ApiResponse<List<GetUpComingHearingDto>>>
    {
        private readonly AppDbContext context;

        public GetUpComingHearingHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<ApiResponse<List<GetUpComingHearingDto>>> Handle(GetUpComingHearingQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;
            var nextThreeDays = today.AddDays(3);
            var result = await context.Followups.Include(f => f.Case)
                .Where(f => f.NextHearingDate.HasValue && f.NextHearingDate.Value.Date >= today && f.NextHearingDate.Value.Date <= nextThreeDays)
                .Select(f =>f.Map()).ToListAsync(cancellationToken);
            if(result == null || result.Count == 0)
                return ApiResponse<List<GetUpComingHearingDto>>.Fail("No upcoming hearings found in the next three days.", System.Net.HttpStatusCode.NotFound);
            return ApiResponse<List<GetUpComingHearingDto>>.Ok(result);

        }
    }
}
