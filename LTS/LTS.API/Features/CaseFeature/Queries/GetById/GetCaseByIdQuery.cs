using LTS.API.Common.Response;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using MediatR;

namespace LTS.API.Features.CaseFeature.Queries.GetById
{
    public record GetCaseByIdQuery(Guid Id) : IRequest<ApiResponse<GetCasesDto>>;
}
