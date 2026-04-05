using FluentValidation;

namespace LTS.API.Features.CaseDocument.Commands.DeleteDocument
{
    public class DeleteCaseDocumentCommandValidator:AbstractValidator<DeleteCaseDocumentCommand>
    {
        public DeleteCaseDocumentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Document Id is Required");
        }
    }
}
