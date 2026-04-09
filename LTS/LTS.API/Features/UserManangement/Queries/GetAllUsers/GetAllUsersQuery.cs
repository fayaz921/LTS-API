using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<ApiResponse<List<GetAllUsersDto>>>;


}
