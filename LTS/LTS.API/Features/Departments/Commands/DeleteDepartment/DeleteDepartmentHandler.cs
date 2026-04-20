using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
namespace LTS.API.Features.Departments.Commands.DeleteDepartment;
public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ApiResponse<string>>
{
    private readonly AppDbContext _context;
    public DeleteDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<string>> Handle(DeleteDepartmentCommand request, CancellationToken ct)
    {
        var department = await _context.Departments.FindAsync(request.DepartmentId);
        if (department == null)
            return ApiResponse<string>.NotFound("Department not found");
        department.IsActive = false;

        var result = await _context.SaveChangesAsync(ct);

        if (result > 0)
        {
            return ApiResponse<string>.Ok(message: "Updated successfully");
        }
        else
        {
            return ApiResponse<string>.Fail("Delete failed");
        }
    }
}