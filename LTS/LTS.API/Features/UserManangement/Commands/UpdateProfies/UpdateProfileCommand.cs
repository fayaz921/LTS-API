using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.UpdateProfies
{
    public record UpdateProfileCommand(
       string Name,
        string Phone,
        string Location
        ) : IRequest<ApiResponse<string>>;


}
