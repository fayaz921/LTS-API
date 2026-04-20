using LTS.API.Common.Response;
using MediatR;

public record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<ApiResponse<string>>;