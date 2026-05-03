using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.SearchCases
{
    public record SearchCasesQuery(string CaseNo, string Cnic, string PetititionerName,int Page,int PageSize) : IRequest<ApiResponse<List<GetCaseDto>>>
    {
        public Guid OrganizationId { get; set; }
    }


}
