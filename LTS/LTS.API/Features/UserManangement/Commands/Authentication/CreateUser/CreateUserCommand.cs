using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public record CreateUserCommand(
     string OrganizationName,
    string OwnerName,
    string Email,
    string Password) :IRequest<ApiResponse<string>>;

}
