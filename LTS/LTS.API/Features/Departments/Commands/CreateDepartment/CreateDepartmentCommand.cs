using MediatR;
using LTS.API.Common.Response;
public record CreateDepartmentCommand(string DepartmentName, string? AddressContact) : IRequest<ApiResponse<Guid>>;