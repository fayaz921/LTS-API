using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Benchs.Commands.UpdateBench;

public record UpdateBenchCommand(Guid BenchId, string JudgeName, string JudgeContactNo, string JudgeEmail
   ) : IRequest<ApiResponse<bool>>;

     
