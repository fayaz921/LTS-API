using LTS.API.Common.Response;
using LTS.API.Features.Reports.DTOs;
using MediatR;

namespace LTS.API.Features.Reports.Queries.GetSummaryReport
{
    public class GetSummaryReportQuery : IRequest<ApiResponse<GetSummaryReportDto>>
    {
        public Guid OrganizationId { get; set; }
    }
}
