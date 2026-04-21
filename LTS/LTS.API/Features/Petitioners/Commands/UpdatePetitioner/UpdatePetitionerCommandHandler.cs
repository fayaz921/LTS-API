using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Petitioners.Commands.UpdatePetitioner
{
    public class UpdatePetitionerCommandHandler : IRequestHandler<UpdatePetitionerCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public UpdatePetitionerCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<string>> Handle(UpdatePetitionerCommand request, CancellationToken cancellationToken)
        {
            var petitioner = await _context.Petitioners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (petitioner is null)
               return ApiResponse<string>.Fail("Petitioner not found", System.Net.HttpStatusCode.NotFound);

          
            petitioner.Name = request.Name;
            petitioner.Address = request.Address;
            petitioner.Phone = request.Phone;
            petitioner.Mobile = request.Mobile;
            petitioner.Business = request.Business;
            petitioner.Email = request.Email;
            petitioner.CNIC = request.CNIC;
            petitioner.UpdatedAt = DateTime.UtcNow;
            petitioner.UpdatedBy = request.UpdatedBy;

            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!,"Petitioner updated successfully");

        }
    }
}
