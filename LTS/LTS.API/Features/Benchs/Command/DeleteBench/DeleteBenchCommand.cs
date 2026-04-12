using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Benchs.Commands.DeleteBench;

public record DeleteBenchCommand(Guid BenchId) : IRequest<ApiResponse<bool>>;

    
