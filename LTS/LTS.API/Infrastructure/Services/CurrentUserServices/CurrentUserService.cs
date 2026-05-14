using System.Security.Claims;

namespace LTS.API.Infrastructure.Services.CurrentUserServices
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; }
        public Guid OrganizationId { get; }
        public string Role { get; }
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
            var user = httpContextAccessor.HttpContext?.User;

            UserId = Guid.TryParse(
                user?.FindFirstValue(ClaimTypes.NameIdentifier), out var uid)
                ? uid : Guid.Empty;

            OrganizationId = Guid.TryParse(
                user?.FindFirstValue("OrganizationId"), out var oid)
                ? oid : Guid.Empty;

            Role = user?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }
    }
}
