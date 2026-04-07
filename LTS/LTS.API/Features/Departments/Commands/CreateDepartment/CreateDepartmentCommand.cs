using MediatR;
namespace LTS.API.Features.Departments.Commands.CreateDepartment;
public record CreateDepartmentCommand(string DepartmentName, string? AddressContact) : IRequest<Guid>;