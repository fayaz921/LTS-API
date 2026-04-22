using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public record GetAllCasesQuery() : IRequest<ApiResponse<List<GetCaseDto>>>;
}

