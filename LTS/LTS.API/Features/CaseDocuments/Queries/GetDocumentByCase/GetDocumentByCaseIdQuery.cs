using LTS.API.Common.Response;
using LTS.API.Features.CaseDocuments.DTOs;
using MediatR;

namespace LTS.API.Features.CaseDocuments.Queries.GetDocumentByCase
{
    public record GetDocumentByCaseIdQuery(Guid caseId) : IRequest<ApiResponse<List<GetCaseDocument>>>;
 
}
