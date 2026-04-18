using LTS.API.Common.Response;
using LTS.API.Features.Departments.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Departments.Queries.GetAllDepartments;
public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, ApiResponse<List<GetAllDepartmentsDto>>>
{
    private readonly AppDbContext _context;
    public GetAllDepartmentsHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<List<GetAllDepartmentsDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
    {
        var departments = await _context.Departments
            .Where(x => x.IsActive).Select(x => new GetAllDepartmentsDto
            {
                Id = x.Id,
                DepartmentName = x.DepartmentName,
                AddressContact = x.AddressContact,
                IsActive = x.IsActive
            }).ToListAsync(ct);

        return ApiResponse<List<GetAllDepartmentsDto>>.Ok(departments);
    }
}