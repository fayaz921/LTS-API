using LTS.API.Common.Response;
using LTS.API.Features.Reports.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Reports.Queries.GetCourtWiseReport;

public class GetCourtWiseReportHandler
    : IRequestHandler<GetCourtWiseReportQuery, ApiResponse<List<GetCourtWiseReportDto>>>
{
    private readonly AppDbContext _context;

    public GetCourtWiseReportHandler(AppDbContext context)
    {
        _context = context;
    }


    
    public async Task<ApiResponse<List<GetCourtWiseReportDto>>> Handle(GetCourtWiseReportQuery query, CancellationToken cancellationToken)
    {
        var report = await _context.Courts
            .Where(c => c.Cases.Any()) // Sirf wo courts jin ke cases hain
            .Select(c => new GetCourtWiseReportDto
            {
                CourtId = c.Id,
                CourtName = c.CourtName,
                TotalCases = c.Cases.Count()
            })
            .OrderByDescending(x => x.TotalCases)
            .ToListAsync(cancellationToken);

        // Check agar list khali hai
        if (report == null || !report.Any())
        {
            return ApiResponse<List<GetCourtWiseReportDto>>.Fail("Abhi tak koi case data available nahi hai.");
        }

        return ApiResponse<List<GetCourtWiseReportDto>>.Ok(report, "Court wise report kamyabi se taiyar hai.");
    }



}