using LTS.API.Common.Response;
using LTS.API.Features.Petitioners.Queries.Dtos;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Petitioners.Queries.GetAllPetitioners
{
    public class GetAllPetitionersHandler : IRequestHandler<GetAllPetitionersQuery, ApiResponse<List<PetitionerResponseDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllPetitionersHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<PetitionerResponseDto>>> Handle(GetAllPetitionersQuery request, CancellationToken cancellationToken)
        {
            var petitioners = await _context.Petitioners
                .Where(x => x.OrganizationId == request.OrganizationId)
                .Select(x => new PetitionerResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Phone = x.Phone,
                    Mobile = x.Mobile,
                    Business = x.Business,
                    Email = x.Email,
                    CNIC = x.CNIC,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new ApiResponse<List<PetitionerResponseDto>>
            {
                Message = "Petitioners fetched successfully",
                Data = petitioners
            };
        }
    }
}