using MediatR;
using LTS.API.Common.Response;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Queries.GetCourtById
{
    public record GetCourtByIdQuery(Guid Id) : IRequest<ApiResponse<CourtDto>>;
}