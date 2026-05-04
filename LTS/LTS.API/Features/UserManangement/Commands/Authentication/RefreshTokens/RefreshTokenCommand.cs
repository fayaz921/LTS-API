using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.RefreshTokens
{
    public record  RefreshTokenCommand : IRequest<ApiResponse<string>>;
}
 