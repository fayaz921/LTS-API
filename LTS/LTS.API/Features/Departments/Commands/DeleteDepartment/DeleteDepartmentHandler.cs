using LTS.API.Infrastructure.Persistence;
using MediatR;
namespace LTS.API.Features.Departments.Commands.DeleteDepartment;
public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly AppDbContext _context;
    public DeleteDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken ct)
    {
        var department = await _context.Departments.FindAsync(request.DepartmentId);
        if (department == null)
            return false;
        // Soft delete
        department.IsActive = false;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}