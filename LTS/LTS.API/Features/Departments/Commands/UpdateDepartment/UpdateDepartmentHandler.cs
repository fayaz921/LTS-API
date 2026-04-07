using LTS.API.Infrastructure.Persistence;
using MediatR;
namespace LTS.API.Features.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, bool>
{
    private readonly AppDbContext _context;
    public UpdateDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken ct)
    {
        var department = await _context.Departments.FindAsync(request.DepartmentId);
        if (department == null)
            return false;
        department.DepartmentName = request.DepartmentName;
        department.AddressContact = request.AddressContact;
        department.IsActive = request.IsActive;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}