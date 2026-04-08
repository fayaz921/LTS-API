using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public record GetAllUserQuery: IRequest<List<GetUserDto>>;

}
