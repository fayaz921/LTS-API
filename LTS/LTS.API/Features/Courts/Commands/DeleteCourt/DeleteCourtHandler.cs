using MediatR;
using System.Net;
using Microsoft.EntityFrameworkCore;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;

namespace LTS.API.Features.Courts.Commands.DeleteCourt
{
    public class DeleteCourtHandler : IRequestHandler<DeleteCourtCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public DeleteCourtHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(DeleteCourtCommand request, CancellationToken ct)
        {
            var court = await _context.Courts.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, ct);
            if (court == null)
                return ApiResponse<string>.NotFound("Court not found"); 
            court.IsActive = false;
            var result = await _context.SaveChangesAsync(ct);
            if (result > 0)
            {
                return ApiResponse<string>.Ok(message: "Court deleted successfully");
            }
            else
            {
                return ApiResponse<string>.Fail("Delete failed");
            }
        }
    }
}