namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public record GetUserDto(Guid UserId,
        string Name,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAt
    );
   
}
