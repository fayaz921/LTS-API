using MediatR;

namespace LTS.API.Features.CaseDocument.Commands.UploadDocument
{
    public record UploadCaseDocumentCommand(Guid caseId, IFormFile File, string? FileName, string? Remarks) : IRequest<string>;
}
