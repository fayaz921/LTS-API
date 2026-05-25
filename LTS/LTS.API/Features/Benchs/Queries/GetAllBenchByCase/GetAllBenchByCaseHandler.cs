
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Benchs.Queries.GetAllBenchByCase
{
   

    public class GetAllBenchByCaseHandler : IRequestHandler<GetAllBenchByCaseQuery, ApiResponse<object>>
    {
        private readonly AppDbContext _context;

        public GetAllBenchByCaseHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> Handle(GetAllBenchByCaseQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Benches
                .Include(x => x.Case)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                //query = query.Where(x =>
               //x.Case.CaseNo.Contains(request.Search) ||
               //x.Case.Title.Contains(request.Search) ||
               //x.JudgeName.Contains(request.Search));
               query = query.Where(x =>
        x.JudgeName.Contains(request.Search) ||
        x.Case.CaseNo.Contains(request.Search));
            }

            var total = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new
                {
                    x.Id,
                    x.JudgeName,
                    x.JudgeEmail,
                    x.JudgeContactNo,
                    x.CreatedAt,

                    CaseNo = x.Case.CaseNo,
                    CaseTitle = x.Case.Title
                })
                .ToListAsync(cancellationToken);

            return ApiResponse<object>.Ok(data);
        }
    
    
    
    
    
    }
}
