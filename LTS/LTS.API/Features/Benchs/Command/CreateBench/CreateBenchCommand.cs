using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Benchs.Commands.CreateBench;

public record  CreateBenchCommand(  Guid CaseId ,string JudgeName, string JudgeContactNo,
    string JudgeEmail) : IRequest<ApiResponse<Guid>>;
