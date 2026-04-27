using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Courts.Commands.CreateCourt
{
    public class CreateCourtHandler : IRequestHandler<CreateCourtCommand, ApiResponse<Guid>>
    {
        private readonly AppDbContext _context;

        public CreateCourtHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateCourtCommand request, CancellationToken ct)
        {
            var court = new Court
            {
                Id = Guid.NewGuid(),
                CourtName = request.CourtName,
                AddressContact = request.AddressContact,
                IsActive = true,
                CreatedAt = DateTime.UtcNow  
            };

            await _context.Courts.AddAsync(court, ct);
            var result = await _context.SaveChangesAsync(ct);

            if (result > 0)
            {
                return ApiResponse<Guid>.Created(court.Id);
            }
            else
            {
                return ApiResponse<Guid>.Fail("Failed to create court");
            }
        }
    }
}