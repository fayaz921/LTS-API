using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;

namespace LTS.API.Features.Courts.Commands.UpdateCourt
{
    public class UpdateCourtHandler : IRequestHandler<UpdateCourtCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public UpdateCourtHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(UpdateCourtCommand request, CancellationToken ct)
        {
            var court = await _context.Courts.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, ct);

            if (court == null)
                return ApiResponse<string>.NotFound("Court not found");

            court.CourtName = request.CourtName;
            court.AddressContact = request.AddressContact;
            court.IsActive = request.IsActive;

            var result = await _context.SaveChangesAsync(ct);

            if (result > 0)
            {
                return ApiResponse<string>.Ok(message: "Court updated successfully");
            }
            else
            {
                return ApiResponse<string>.Fail("Update failed");
            }
        }
    }
}