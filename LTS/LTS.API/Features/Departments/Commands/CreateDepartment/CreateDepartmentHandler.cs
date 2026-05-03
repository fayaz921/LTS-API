using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
namespace LTS.API.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, ApiResponse<Guid>>
{
    private readonly AppDbContext _context;
    public CreateDepartmentHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            DepartmentName = request.DepartmentName,
            AddressContact = request.AddressContact,
            IsActive = true,
            OrganizationId = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a"),// later we send it dynamicaly
            CreatedAt = DateTime.UtcNow
        };

        await _context.Departments.AddAsync(department, ct);
        var result = await _context.SaveChangesAsync(ct);

        if (result > 0)
        {
            return ApiResponse<Guid>.Created(department.Id);
        }
        else
        {
            return ApiResponse<Guid>.Fail("Failed to create department");
        }
    }
}