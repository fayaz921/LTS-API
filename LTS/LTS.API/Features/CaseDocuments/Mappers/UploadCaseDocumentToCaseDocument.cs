using LTS.API.Domain.Entities;
using LTS.API.Features.CaseDocuments.Commands.UploadDocument;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;

namespace LTS.API.Features.CaseDocuments.Mappers
{
    public static class UploadCaseDocumentToCaseDocument
    {
        public static CaseDocument Map(this UploadCaseDocumentCommand uploadCaseDocumentCommand, FileUploadResult result)
        {
            return new CaseDocument
            {
                Id = Guid.NewGuid(),
                CaseId = uploadCaseDocumentCommand.caseId,
                FileName = uploadCaseDocumentCommand.FileName ?? uploadCaseDocumentCommand.File.FileName,
                FilePath = result.Url,
                FileType = uploadCaseDocumentCommand.File.ContentType,
                FileSize = uploadCaseDocumentCommand.File.Length,
                Remarks = uploadCaseDocumentCommand.Remarks,
                PublicId = result.PublicId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
