using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Petitioners.Commands.CreatePetitioner
{
    public class CreatePetitionerCommandHandler : IRequestHandler<CreatePetitionerCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public CreatePetitionerCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(CreatePetitionerCommand request, CancellationToken cancellationToken)
        {

            var exists = await _context.Petitioners
         .AnyAsync(x => x.CNIC == request.CNIC, cancellationToken);

            if (exists)
               return ApiResponse<string>.Fail("A petitioner with the same CNIC already exists.", System.Net.HttpStatusCode.Conflict);

            var petitioner = new Petitioner
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Business = request.Business,
                Email = request.Email,
                CNIC = request.CNIC,
                OrganizationId = request.OrganizationId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
            };

            await _context.Petitioners.AddAsync(petitioner, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Created(petitioner.Id.ToString(), "Petitioner created successfully");

        }
    }
}
