using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Lognouts
{
    public class LogoutCommand : IRequest<ApiResponse<string>>;
}
