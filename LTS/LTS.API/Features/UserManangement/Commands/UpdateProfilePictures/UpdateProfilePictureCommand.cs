using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.UserManangement.Commands.UpdateProfilePictures
{
    public class UpdateProfilePictureCommand : IRequest<ApiResponse<string>>
    {
        public IFormFile File { get; set; } = null!;
    }
}
