using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.Reports.DTOs;
using LTS.API.Features.Reports.Queries.GetDepartmentWiseReport;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Reports.Queries.GetSummaryReport
{
    public class GetSummaryReportHandler : IRequestHandler<GetSummaryReportQuery, ApiResponse<GetSummaryReportDto>>
    {
        private readonly AppDbContext _context;

        public GetSummaryReportHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<GetSummaryReportDto>> Handle(GetSummaryReportQuery request, CancellationToken cancellationToken)
        {
            var stats = await _context.Cases
                .Where(c => c.OrganizationId == request.OrganizationId)
                .GroupBy(c => 1)
                .Select(g => new GetSummaryReportDto(
                    g.Count(),
                    g.Count(c => c.Status == CaseStatus.Finalized),
                    g.Count(c => c.Status == CaseStatus.Pending)
                ))
                .FirstOrDefaultAsync(cancellationToken);

                return stats != null ? ApiResponse<GetSummaryReportDto>.Ok(stats): ApiResponse<GetSummaryReportDto>.Fail("No Data Found !");
        }
    }
}