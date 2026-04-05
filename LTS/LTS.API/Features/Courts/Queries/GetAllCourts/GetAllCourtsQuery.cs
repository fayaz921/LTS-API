using MediatR;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Queries.GetAllCourts;

public record GetAllCourtsQuery() : IRequest<List<GetAllCourtsDto>>;