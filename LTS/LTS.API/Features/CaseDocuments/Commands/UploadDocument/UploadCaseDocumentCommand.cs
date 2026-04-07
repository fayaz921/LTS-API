using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseDocuments.Commands.UploadDocument
{
    public record UploadCaseDocumentCommand(Guid caseId, IFormFile File, string? FileName, string? Remarks) : IRequest<ApiResponse<string>>;
}
