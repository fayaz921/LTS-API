using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Benchs.Commands.UpdateBench;

public class UpdateBenchHandler : IRequestHandler<UpdateBenchCommand, ApiResponse<bool>>
{
    private readonly AppDbContext _context;

    public UpdateBenchHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateBenchCommand command, CancellationToken cancellationToken)
    {
        var bench = await _context.Benches
            .FirstOrDefaultAsync(b => b.Id == command.BenchId, cancellationToken);

        if (bench is null)
            return ApiResponse<bool>.Fail("Bench nahi mila.");

        bench.JudgeName = command.JudgeName;
        bench.JudgeContactNo = command.JudgeContactNo;
        bench.JudgeEmail = command.JudgeEmail;
        //bench.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok(true, "Bench successfully update ho gaya.");
    }
}






