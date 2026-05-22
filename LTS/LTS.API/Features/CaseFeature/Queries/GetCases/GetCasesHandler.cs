using CloudinaryDotNet.Core;
using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public class GetCasesHandler(AppDbContext context)
        : IRequestHandler<GetAllCasesQuery, ApiResponse<PagedResult<GetCaseDto>>>
    {
        private readonly AppDbContext _context = context;

        public async Task<ApiResponse<PagedResult<GetCaseDto>>> Handle(
            GetAllCasesQuery request, CancellationToken ct)
        {
            try
            {
                var query = BuildQuery(request);

                var totalCount = await query.CountAsync(ct);
                if (totalCount == 0)
                    return ApiResponse<PagedResult<GetCaseDto>>.Ok("Case Table is Empty");

                var cases = await FetchPageAsync(query, request, ct);

                return ApiResponse<PagedResult<GetCaseDto>>.Ok(ToPagedResult(cases, totalCount, request));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetCaseDto>>
                    .Fail($"Internal server error: {ex.Message}");
            }
        }

        #region private helpers 

        private IQueryable<Case> BuildQuery(GetAllCasesQuery request) =>
            _context.Cases
                .Where(c => c.OrganizationId == request.OrganizationId)
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt);

        private static async Task<List<GetCaseDto>> FetchPageAsync(
               IQueryable<Case> query, GetAllCasesQuery request, CancellationToken ct) =>
               await query
                   .Skip((request.Page - 1) * request.PageSize)
                   .Take(request.PageSize)
                   .Select(c => new GetCaseDto(
                       c.Id,
                       c.CaseNo,
                       c.Title,
                       c.Subject,
                       c.DAG,
                       c.Status.ToString(),
                       c.DateInstitution,
                       c.Court != null ? c.Court.CourtName : "N/A",
                       c.Department != null ? c.Department.DepartmentName : "N/A",
                       c.CasePetitioners.Select(cp => new PetitionerDetailDto(
                           cp.Petitioner.Id,
                           cp.Petitioner.Name,
                           cp.Petitioner.CNIC,
                           cp.Petitioner.Email,
                           cp.Petitioner.Phone
                       )).ToList()
                   ))
                   .ToListAsync(ct);
                  


        private static PagedResult<GetCaseDto> ToPagedResult(
            List<GetCaseDto> cases, int totalCount, GetAllCasesQuery request) =>
            new()
            {
                Items = cases,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        #endregion
    }
}