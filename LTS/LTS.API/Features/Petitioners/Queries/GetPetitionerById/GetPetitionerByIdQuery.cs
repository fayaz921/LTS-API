using LTS.API.Common.Response;
using LTS.API.Features.Petitioners.Queries.Dtos;
using MediatR;

namespace LTS.API.Features.Petitioners.Queries.GetPetitionerById
{
    public record GetPetitionerByIdQuery(Guid Id) : IRequest<ApiResponse<PetitionerResponseDto>>;
}
