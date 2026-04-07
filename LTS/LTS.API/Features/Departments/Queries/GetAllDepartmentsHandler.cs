using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Features.Departments.DTOs;

namespace LTS.API.Features.Departments.Queries.GetAllDepartments;
public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, List<GetAllDepartmentsDto>>
{
    private readonly AppDbContext _context;
    public GetAllDepartmentsHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<GetAllDepartmentsDto>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
    {
        return await _context.Departments
            .Where(x => x.IsActive).Select(x => new GetAllDepartmentsDto
            {
                Id = x.Id,
                DepartmentName = x.DepartmentName,
                AddressContact = x.AddressContact,
                IsActive = x.IsActive
            }).ToListAsync(ct);
    }
}