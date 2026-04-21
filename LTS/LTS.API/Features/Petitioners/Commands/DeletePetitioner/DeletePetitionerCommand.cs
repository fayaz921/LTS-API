using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Petitioners.Commands.DeletePetitioner
{
    public record DeletePetitionerCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
