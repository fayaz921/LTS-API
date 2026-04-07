using LTS.API.Domain.Enums;

namespace LTS.API.Features.UserManangement.DTOs
{
    public record RegisterOrganizationDto(
    string OrganizationName,
    SubscriptionPlan SubscriptionPlan,
    string OwnerName,
    string Email,
    string Password);
    
}
