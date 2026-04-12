using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Benchs.Commands.DeleteBench;

public class DeleteBenchHandler : IRequestHandler<DeleteBenchCommand, ApiResponse<bool>>
{
    private readonly AppDbContext _context;

    public DeleteBenchHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteBenchCommand command, CancellationToken cancellationToken)
    {
        var bench = await _context.Benches
            .FirstOrDefaultAsync(b => b.Id == command.BenchId, cancellationToken);

        if (bench is null)
            return ApiResponse<bool>.Fail("Bench nahi mila.");

        _context.Benches.Remove(bench);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Ok("Bench successfully delete ho gaya.");
    }
}