namespace LTS.API.Domain.Features.UserManangement.Queries.GetUsers
{
    public record UserGetDto(Guid UserId,
        string Name,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAt
    );
   
}
