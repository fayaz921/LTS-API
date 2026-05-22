using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.SearchCases
{
    public class SearchCasesQuery : IRequest<ApiResponse<PagedResult<GetCaseDto>>>
    {
        public string? SearchTerm { get; set; }   
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        // Filters (optional)
        public string? Status { get; set; }  
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid OrganizationId { get; set; }
    }


}
