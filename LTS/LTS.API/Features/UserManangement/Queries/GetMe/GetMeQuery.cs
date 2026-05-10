using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetMe
{
    public record GetMeQuery() : IRequest<ApiResponse<GetMeDto>>;
    
}
