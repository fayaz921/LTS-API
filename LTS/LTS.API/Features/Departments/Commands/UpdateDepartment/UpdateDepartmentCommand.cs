using LTS.API.Common.Response;
using MediatR;
namespace LTS.API.Features.Departments.Commands.UpdateDepartment;
public record UpdateDepartmentCommand(Guid DepartmentId,string DepartmentName,string? AddressContact) : IRequest<ApiResponse<string>>;