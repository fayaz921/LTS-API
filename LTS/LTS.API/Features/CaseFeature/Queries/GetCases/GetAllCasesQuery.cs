using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public record GetAllCasesQuery() : IRequest<ApiResponse<PagedResult<GetCaseDto>>>
    {
        public Guid OrganizationId { get; set; } = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a"); // later we update it 
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

}

