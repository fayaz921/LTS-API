using LTS.API.Common.Response;
using LTS.API.Features.Bench.Mappings;
using LTS.API.Features.Benchs.Commands.CreateBench;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Bench.Commands.CreateBench;

public class CreateBenchHandler : IRequestHandler<CreateBenchCommand, ApiResponse<Guid>>
{
    private readonly AppDbContext _context;

    public CreateBenchHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateBenchCommand command, CancellationToken cancellationToken)
    {
        // CaseId validate karo
        var caseExists = await _context.Cases
            .AnyAsync(c => c.Id == command.CaseId, cancellationToken);

        if (!caseExists)
            return ApiResponse<Guid>.Fail("Case nahi mila.");

        var bench = command.ToEntity();

        await _context.Benches.AddAsync(bench, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Created(bench.Id, "Bench successfully create ho gaya.");
    }
}