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

        department.DepartmentName = request.DepartmentName;
        department.AddressContact = request.AddressContact;
        department.IsActive = request.IsActive;

        var result = await _context.SaveChangesAsync(ct);

        if (result > 0)
        {
            return ApiResponse<string>.Ok(message: "Operation successful");
        }
        else
        {
            return ApiResponse<string>.Fail("Update failed");
        }
    }
}