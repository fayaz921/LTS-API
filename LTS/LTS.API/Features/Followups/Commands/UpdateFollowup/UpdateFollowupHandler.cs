using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;

namespace LTS.API.Features.Followups.Commands.UpdateFollowup
{
    public class UpdateFollowupHandler : IRequestHandler<UpdateFollowupCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public UpdateFollowupHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<string>> Handle(UpdateFollowupCommand request, CancellationToken cancellationToken)
        {
            var followup = await _context.Followups.FindAsync(request.FollowupId);

            if (followup is null)
                return ApiResponse<string>.Fail("Followup not found.");

            followup.HearingDate = request.HearingDate;
            followup.NextHearingDate = request.NextHearingDate;
            followup.InterimOrder = request.InterimOrder;
            followup.Decision = request.Decision;
            followup.Remarks = request.Remarks;
            followup.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!,"Followup updated successfully.");
        }
    }
}
