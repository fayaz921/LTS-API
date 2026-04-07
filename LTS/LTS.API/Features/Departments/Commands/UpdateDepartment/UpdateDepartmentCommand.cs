using MediatR;
namespace LTS.API.Features.Departments.Commands.UpdateDepartment;
public record UpdateDepartmentCommand(Guid DepartmentId,string DepartmentName,string? AddressContact,bool IsActive) : IRequest<bool>;