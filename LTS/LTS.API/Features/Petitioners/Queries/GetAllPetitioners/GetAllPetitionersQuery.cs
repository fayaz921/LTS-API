using LTS.API.Common.Response;
using LTS.API.Features.Petitioners.Queries.Dtos;
using MediatR;

namespace LTS.API.Features.Petitioners.Queries.GetAllPetitioners
{
    public record GetAllPetitionersQuery(Guid OrganizationId) : IRequest<ApiResponse<List<PetitionerResponseDto>>>;
}
