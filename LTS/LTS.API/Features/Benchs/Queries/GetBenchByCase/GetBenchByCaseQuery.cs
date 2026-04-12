using LTS.API.Common.Response;
using LTS.API.Features.Bench.DTOs;
using MediatR;

namespace LTS.API.Features.Bench.Queries.GetBenchByCase;

public record GetBenchByCaseQuery(Guid CaseId) : IRequest<ApiResponse<List<GetBenchByCaseDto>>>;

      
