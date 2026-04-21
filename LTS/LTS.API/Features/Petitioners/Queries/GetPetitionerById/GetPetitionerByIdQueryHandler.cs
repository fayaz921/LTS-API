using LTS.API.Common.Response;
using LTS.API.Features.Petitioners.Queries.Dtos;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Petitioners.Queries.GetPetitionerById
{
    public class GetPetitionerByIdQueryHandler : IRequestHandler<GetPetitionerByIdQuery, ApiResponse<PetitionerResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetPetitionerByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PetitionerResponseDto>> Handle(GetPetitionerByIdQuery request, CancellationToken cancellationToken)
        {
            var petitioner = await _context.Petitioners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (petitioner is null)
                return ApiResponse<PetitionerResponseDto>.NotFound("Petitioner not found");

            return new ApiResponse<PetitionerResponseDto>
            {
                Message = "Petitioner fetched successfully",
                Data = new PetitionerResponseDto
                {
                    Id = petitioner.Id,
                    Name = petitioner.Name,
                    Address = petitioner.Address,
                    Phone = petitioner.Phone,
                    Mobile = petitioner.Mobile,
                    Business = petitioner.Business,
                    Email = petitioner.Email,
                    CNIC = petitioner.CNIC,
                    CreatedAt = petitioner.CreatedAt
                }
            };
        }
    }
}
