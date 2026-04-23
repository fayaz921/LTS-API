using LTS.API.Common.Exceptions;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Followups.Commands.CreateFollowup
{
    public class CreateFollowupHandler : IRequestHandler<CreateFollowupCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public CreateFollowupHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(CreateFollowupCommand request, CancellationToken cancellationToken)
        {
            var caseExists = await _context.Cases
              .AnyAsync(c => c.Id == request.CaseId, cancellationToken);

            if (!caseExists)
               return  ApiResponse<string>.Fail($"Case with Id {request.CaseId} not found.");

            var followup = request.ToEntity();

            await _context.Followups.AddAsync(followup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,"Data saved successfully.");
        }
    }
}
