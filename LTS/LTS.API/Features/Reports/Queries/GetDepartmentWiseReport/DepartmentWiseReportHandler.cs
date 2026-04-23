using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.Reports.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Reports.Queries.GetDepartmentWiseReport
{
    public class DepartmentWiseReportHandler(AppDbContext dbContext) : IRequestHandler<GetDepartmentWiseReportQuery, ApiResponse<List<GetDepartmentWiseReportDto>>>
    {
        private readonly AppDbContext _context = dbContext;
        public async Task<ApiResponse<List<GetDepartmentWiseReportDto>>> Handle(GetDepartmentWiseReportQuery request, CancellationToken cancellationToken)
        {
            var report = _context.Departments.Where(d => d.OrganizationId == request.OrganizationId && d.IsActive).Select(d => new GetDepartmentWiseReportDto(
                d.Id,
                d.DepartmentName,
                d.Cases.Count(),
                d.Cases.Count(c => c.Status == CaseStatus.Finalized),
                d.Cases.Count(c => c.Status == CaseStatus.Pending)
            )).ToList();
            return ApiResponse<List<GetDepartmentWiseReportDto>>.Ok(report);
        }
    }
}
