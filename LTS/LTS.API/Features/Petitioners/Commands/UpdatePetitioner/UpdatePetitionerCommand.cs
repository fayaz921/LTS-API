using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Petitioners.Commands.UpdatePetitioner
{
    public record UpdatePetitionerCommand(
        Guid Id,
        string Name,
        string? Address,
        string? Phone,
        string? Mobile,
        string? Business,
        string? Email,
        string? CNIC, 
        string UpdatedBy
    ) : IRequest<ApiResponse<string>>;
}
