using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetUsers
{
    public record GetAllUserQuery: IRequest<List<UserGetDto>>;

}
