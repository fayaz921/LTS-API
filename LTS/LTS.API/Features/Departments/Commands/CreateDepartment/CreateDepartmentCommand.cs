using MediatR;
using LTS.API.Common.Response;
namespace LTS.API.Features.Departments.Commands.CreateDepartment;
public record CreateDepartmentCommand(string DepartmentName, string? AddressContact) : IRequest<ApiResponse<Guid>>;