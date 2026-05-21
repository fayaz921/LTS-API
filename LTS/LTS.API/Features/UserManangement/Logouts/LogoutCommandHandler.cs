using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Logouts
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenservice;

        public LogoutCommandHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor,ITokenService tokenservice)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this._tokenservice = tokenservice;
        }

        public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext!;
            var refreshToken = context.Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var decoded = Uri.UnescapeDataString(refreshToken);
                await _context.RefreshTokens
                 .Where(x => x.Token == _tokenservice.HashToken(decoded) && !x.IsRevoked)
                 .ExecuteUpdateAsync(s => s
                     .SetProperty(x => x.IsRevoked, true),
                     cancellationToken);          
            }
            context.Response.Cookies.Delete("refreshToken");
            return ApiResponse<string>.Ok(default!,"Logged out successfully");
        }
    }
}
