using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Logouts
{
    public class LogoutCommand : IRequest<ApiResponse<string>>;
}
