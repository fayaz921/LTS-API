using CloudinaryDotNet.Core;
using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public class GetCasesHandler(AppDbContext context)
        : IRequestHandler<GetAllCasesQuery, ApiResponse<CasesPaginatedResult<GetCaseDto>>>
    {
        private readonly AppDbContext _context = context;

        public async Task<ApiResponse<CasesPaginatedResult<GetCaseDto>>> Handle(
            GetAllCasesQuery request, CancellationToken ct)
        {
            try
            {
                var query = BuildQuery(request);

                var totalCount = await query.CountAsync(ct);
                if (totalCount == 0)
                    return ApiResponse<CasesPaginatedResult<GetCaseDto>>.Ok("Case Table is Empty");

                var pendingCount = await query.CountAsync(c => c.Status == CaseStatus.Pending, ct);
                var finalizedCount = await query.CountAsync(c => c.Status == CaseStatus.Finalized, ct);

                var cases = await FetchPageAsync(query, request, ct);

                return ApiResponse<CasesPaginatedResult<GetCaseDto>>.Ok(ToPagedResult(cases, totalCount,pendingCount,finalizedCount, request));
            }
            catch (Exception ex)
            {
                return ApiResponse<CasesPaginatedResult<GetCaseDto>>
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
                  


        private static CasesPaginatedResult<GetCaseDto> ToPagedResult(
            List<GetCaseDto> cases, int totalCount,int pendingCount,int finalizedCount,GetAllCasesQuery request) =>
            new()
            {
                Items = cases,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize,
                PendingCount=pendingCount,
                FinalizedCount=finalizedCount
            };
        #endregion
    }
}