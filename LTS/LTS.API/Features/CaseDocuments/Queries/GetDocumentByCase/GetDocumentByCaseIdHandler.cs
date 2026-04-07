using LTS.API.Common.Response;
using LTS.API.Features.CaseDocuments.DTOs;
using LTS.API.Features.CaseDocuments.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseDocuments.Queries.GetDocumentByCase
{
    public class GetDocumentByCaseIdHandler : IRequestHandler<GetDocumentByCaseIdQuery, ApiResponse<List<GetCaseDocument>>>
    {
        private readonly AppDbContext context;

        public GetDocumentByCaseIdHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<ApiResponse<List<GetCaseDocument>>> Handle(GetDocumentByCaseIdQuery request, CancellationToken cancellationToken)
        {
            var documents =await context.CaseDocuments.Where(x=>x.CaseId==request.caseId).OrderByDescending(x=>x.CreatedAt).ToListAsync(cancellationToken);
            if (documents == null || documents.Count == 0)
            {
                return ApiResponse<List<GetCaseDocument>>.Fail("No documents found for the specified case", HttpStatusCode.NotFound);
            }
            return ApiResponse<List<GetCaseDocument>>.Ok(documents.MapList(), "Success");
        }
    }
}
