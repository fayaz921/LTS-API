using LTS.API.Common.Response;
using LTS.API.Features.Departments.DTOs;
using LTS.API.Features.Departments.Mappings;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Departments.Queries.GetAllDepartments;
public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, ApiResponse<List<DepartmentDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllDepartmentsHandler(AppDbContext context)
        {
            _context = context;
        }
public async Task<ApiResponse<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
    {
        var departments = await _context.Departments
            .Where(x => x.IsActive)
            .Select(x => x.ToDto())  
            .ToListAsync(ct);

        return ApiResponse<List<DepartmentDto>>.Ok(departments);
    }
}