using LTS.API.Domain.Entities;
using LTS.API.Features.Benchs.Commands.CreateBench;
using LTS.API.Features.Bench.DTOs;

namespace LTS.API.Features.Bench.Mappings;

public static class BenchMappings
{
    public static Domain.Entities.Bench ToEntity(this CreateBenchCommand command)
    {
        return new Domain.Entities.Bench
        {
            Id = Guid.NewGuid(),
            CaseId = command.CaseId,
            JudgeName = command.JudgeName,
            JudgeContactNo = command.JudgeContactNo,
            JudgeEmail = command.JudgeEmail,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static GetBenchByCaseDto ToDto(this Domain.Entities.Bench bench)
    {
        return new GetBenchByCaseDto
        {
            Id = bench.Id,
            CaseId = bench.CaseId,
            JudgeName = bench.JudgeName,
            JudgeContactNo = bench.JudgeContactNo,
            JudgeEmail = bench.JudgeEmail,
            CreatedAt = bench.CreatedAt
        };
    }
}