using FluentValidation;

namespace LTS.API.Features.CaseDocuments.Commands.UploadDocument
{
    public class UploadCaseDocumentCommandValidator:AbstractValidator<UploadCaseDocumentCommand>
    {
        public UploadCaseDocumentCommandValidator()
        {
            RuleFor(x => x.caseId).NotEmpty().WithMessage("CaseId is Required");
            RuleFor(x => x.File).NotNull().WithMessage("File is required");
        }
    }
}
