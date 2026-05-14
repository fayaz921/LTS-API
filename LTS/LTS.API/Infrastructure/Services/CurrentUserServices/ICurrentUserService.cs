using LTS.API.Domain.Enums;

namespace LTS.API.Infrastructure.Services.CurrentUserServices
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Guid OrganizationId { get; }
        string Role { get; }
    }
}
