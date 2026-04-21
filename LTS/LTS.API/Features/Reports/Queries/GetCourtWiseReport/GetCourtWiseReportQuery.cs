using LTS.API.Common.Response;
using LTS.API.Features.Reports.DTOs;
using MediatR;

namespace LTS.API.Features.Reports.Queries.GetCourtWiseReport
{
    public record GetCourtWiseReportQuery()
     : IRequest<ApiResponse<List<GetCourtWiseReportDto>>>;
}
