using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Petitioners.Commands.DeletePetitioner
{
    public class DeletePetitionerHandler : IRequestHandler<DeletePetitionerCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public DeletePetitionerHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(DeletePetitionerCommand request, CancellationToken cancellationToken)
        {
            var petitioner = await _context.Petitioners
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (petitioner is null)
              return ApiResponse<string>.Fail("Petitioner not found", System.Net.HttpStatusCode.NotFound);

            _context.Petitioners.Remove(petitioner);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(petitioner.Id.ToString(), "Petitioner deleted successfully");
        }
    }
}
