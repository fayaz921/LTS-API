using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetProfiles
{
    public class GetProfileQuery : IRequest<ApiResponse<GetProfileDto>>
    {
    }
}
