using LTS.API.Common.Response;
using LTS.API.Features.CaseDocuments.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace LTS.API.Features.CaseDocuments.Queries.GetDocumentById
{
    public record GetDocumentByIdQuery(Guid Id) : IRequest<ApiResponse<GetCaseDocument>>;
}
