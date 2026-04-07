using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.CaseDocuments.DTOs;
using LTS.API.Features.CaseDocuments.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseDocuments.Queries.GetDocumentById
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, ApiResponse<GetCaseDocument>>
    {
        private readonly AppDbContext context;

        public GetDocumentByIdHandler(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<ApiResponse<GetCaseDocument>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await context.CaseDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (document == null)
            {
                return ApiResponse<GetCaseDocument>.Fail("Document not found",HttpStatusCode.NotFound);
            }
            return ApiResponse<GetCaseDocument>.Ok(document.Map(),"Success");
        }
    }
}
