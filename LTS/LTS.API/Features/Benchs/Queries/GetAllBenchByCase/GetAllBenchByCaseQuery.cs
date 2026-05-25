using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Benchs.Queries.GetAllBenchByCase
{
    public record GetAllBenchByCaseQuery(int Page = 1, int PageSize = 10, string? Search = null) : IRequest<ApiResponse<object>>;


}
