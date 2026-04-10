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
            var court = await _context.Courts.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (court == null)
                return ApiResponse<string>.Fail("Court not found", HttpStatusCode.NotFound);

            court.IsActive = false;

            var result = await _context.SaveChangesAsync(ct);

            return result > 0
                ? ApiResponse<string>.Ok(data: "Court updated successfully", message: "Success")
                : ApiResponse<string>.Fail("Delete failed");
        }
    }
}