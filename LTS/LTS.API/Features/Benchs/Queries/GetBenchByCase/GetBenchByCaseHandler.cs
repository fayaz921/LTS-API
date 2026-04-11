using LTS.API.Common.Response;
using LTS.API.Features.Bench.DTOs;
using LTS.API.Features.Bench.Mappings;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Bench.Queries.GetBenchByCase;

public class GetBenchByCaseHandler : IRequestHandler<GetBenchByCaseQuery, ApiResponse<List<GetBenchByCaseDto>>>
{
    private readonly AppDbContext _context;

    public GetBenchByCaseHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<GetBenchByCaseDto>>> Handle(GetBenchByCaseQuery query, CancellationToken cancellationToken)
    {
        var benches = await _context.Benches
            .Where(b => b.CaseId == query.CaseId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);

        if (!benches.Any())
            return ApiResponse<List<GetBenchByCaseDto>>.Fail("Is case ki koi bench entries nahi mili.");

        var result = benches.Select(b => b.ToDto()).ToList();

        return ApiResponse<List<GetBenchByCaseDto>>.Ok(result, "Bench entries successfully mil gayi.");
    }
}