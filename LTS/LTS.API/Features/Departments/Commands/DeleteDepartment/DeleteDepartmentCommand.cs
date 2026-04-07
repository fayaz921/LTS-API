using MediatR;
namespace LTS.API.Features.Departments.Commands.DeleteDepartment;
public record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<bool>;