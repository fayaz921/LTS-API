using MediatR;
using LTS.API.Features.Departments.DTOs;

namespace LTS.API.Features.Departments.Queries.GetAllDepartments;
public record GetAllDepartmentsQuery() : IRequest<List<GetAllDepartmentsDto>>;