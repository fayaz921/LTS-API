using MediatR;
using Microsoft.EntityFrameworkCore;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Features.Departments.DTOs;
using LTS.API.Features.Departments.Mappings;

namespace LTS.API.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdHandler
        : IRequestHandler<GetDepartmentByIdQuery, ApiResponse<DepartmentDto>>
    {
        private readonly AppDbContext _context;

        public GetDepartmentByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
        {
            var department = await _context.Departments
                .Where(x => x.Id == request.Id && x.IsActive)
                .Select(x => x.ToDto())
                .FirstOrDefaultAsync(ct);

            if (department == null)
                return ApiResponse<DepartmentDto>.NotFound("Department not found");

            return ApiResponse<DepartmentDto>.Ok(department);
        }
    }
}