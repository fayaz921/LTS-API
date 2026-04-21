using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using MediatR;

namespace LTS.API.Features.Petitioners.Commands.CreatePetitioner
{
    public record CreatePetitionerCommand(string Name,
        string? Address,
        string? Phone,
        string? Mobile,
        string? Business,
        string?CNIC,
        string? Email,
        Guid OrganizationId,
         string CreatedBy
    ) : IRequest<ApiResponse<string>>;
}
