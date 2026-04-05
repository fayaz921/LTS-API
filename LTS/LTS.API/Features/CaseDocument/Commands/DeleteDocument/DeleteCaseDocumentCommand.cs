using MediatR;

namespace LTS.API.Features.CaseDocument.Commands.DeleteDocument
{
    public record DeleteCaseDocumentCommand(Guid Id):IRequest<string>;

}
