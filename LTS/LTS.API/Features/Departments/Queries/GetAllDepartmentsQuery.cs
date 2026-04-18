using MediatR;
using LTS.API.Features.Departments.DTOs;
using LTS.API.Common.Response;

namespace LTS.API.Features.Departments.Queries.GetAllDepartments;
public record GetAllDepartmentsQuery(): IRequest<ApiResponse<List<GetAllDepartmentsDto>>>;