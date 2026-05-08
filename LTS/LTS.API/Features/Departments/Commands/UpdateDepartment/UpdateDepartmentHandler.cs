using LTS.API.Infrastructure.Persistence;
using MediatR;
using LTS.API.Common.Response;
namespace LTS.API.Features.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, ApiResponse<string>>
{
    private readonly AppDbContext _context;
    public UpdateDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<string>> Handle(UpdateDepartmentCommand request, CancellationToken ct)
    {
        var department = await _context.Departments.FindAsync(new object[] { request.DepartmentId }, ct);
        if (department == null)
            return ApiResponse<string>.NotFound("Department not found");

        if (!department.IsActive)
            return ApiResponse<string>.Fail("Cannot update an inactive department");

        department.DepartmentName = request.DepartmentName;
        department.AddressContact = request.AddressContact;

        var result = await _context.SaveChangesAsync(ct);

        if (result > 0)
        {
            return ApiResponse<string>.Ok(message: "Updated successfully");
        }
        else
            return ApiResponse<string>.Fail("Update failed");
    }
}