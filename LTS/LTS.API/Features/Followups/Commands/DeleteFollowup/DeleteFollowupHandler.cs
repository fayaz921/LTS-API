using LTS.API.Common.Exceptions;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LTS.API.Features.Followups.Commands.DeleteFollowup
{
    public class DeleteFollowupHandler : IRequestHandler<DeleteFollowupCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public DeleteFollowupHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<string>> Handle(DeleteFollowupCommand request, CancellationToken cancellationToken)
        {
            var followup = await _context.Followups.FindAsync(request.id);

            if (followup is null)
                throw new NotFoundException($"Followup with Id {request.id} not found.");

            _context.Followups.Remove(followup);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!, "Followup deleted successfully.");
        }
    }
}
