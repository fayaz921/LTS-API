using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Departments.Commands.DeleteDepartment;
public record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<ApiResponse<string>>;