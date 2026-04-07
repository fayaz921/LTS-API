using MediatR;

namespace LTS.API.Features.CaseDocumentFeature.Commands.UploadDocument
{
    public record UploadCaseDocumentCommand(Guid caseId, IFormFile File, string? FileName, string? Remarks) : IRequest<string>;
}
