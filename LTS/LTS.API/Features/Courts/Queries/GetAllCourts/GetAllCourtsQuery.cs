using MediatR;
using LTS.API.Common.Response;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Queries.GetAllCourts
{
    public record GetAllCourtsQuery(bool? IsActive = null) : IRequest<ApiResponse<List<CourtDto>>>;
}