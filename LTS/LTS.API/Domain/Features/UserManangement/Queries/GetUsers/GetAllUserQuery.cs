using MediatR;

namespace LTS.API.Domain.Features.UserManangement.Queries.GetUsers
{
    public record GetAllUserQuery: IRequest<List<UserGetDto>>;

}
