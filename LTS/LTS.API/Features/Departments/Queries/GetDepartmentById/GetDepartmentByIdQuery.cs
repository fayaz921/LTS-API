using MediatR;
using LTS.API.Common.Response;
using LTS.API.Features.Departments.DTOs;

namespace LTS.API.Features.Departments.Queries.GetDepartmentById;
    public record GetDepartmentByIdQuery(Guid Id) : IRequest<ApiResponse<DepartmentDto>>;