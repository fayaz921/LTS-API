using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Lognouts
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext!;
            var refreshToken = context.Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                // DB mein revoke karo
                var stored = await _db.RefreshTokens
                    .FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked, cancellationToken);

                if (stored != null)
                {
                    stored.IsRevoked = true;
                    await _db.SaveChangesAsync(cancellationToken);
                }

                // Cookie delete karo
                context.Response.Cookies.Delete("refreshToken");
            }

            return ApiResponse<string>.Ok(default!,"Logged out successfully");
        }
    }
}
