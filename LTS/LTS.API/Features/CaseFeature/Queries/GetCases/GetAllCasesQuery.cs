using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public record GetAllCasesQuery() : IRequest<ApiResponse<PagedResult<GetCaseDto>>>
    {
        public Guid OrganizationId { get; set; } 
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

}

