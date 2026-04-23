using LTS.API.Common.Response;
using LTS.API.Features.Reports.DTOs;
using MediatR;

namespace LTS.API.Features.Reports.Queries.GetDepartmentWiseReport
{
    public class GetDepartmentWiseReportQuery : IRequest<ApiResponse<List<GetDepartmentWiseReportDto>>>
    {
        public Guid OrganizationId { get; set; }
    }
}