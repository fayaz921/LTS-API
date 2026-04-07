using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
namespace LTS.API.Features.Departments.Commands.CreateDepartment;
public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly AppDbContext _context;
    public CreateDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var department = new Department
        {
            DepartmentName = request.DepartmentName,
            AddressContact = request.AddressContact
        };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(ct);
        return department.Id;
    }
}