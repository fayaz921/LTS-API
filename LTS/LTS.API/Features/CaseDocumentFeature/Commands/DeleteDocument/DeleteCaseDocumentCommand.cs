using MediatR;

namespace LTS.API.Features.CaseDocumentFeature.Commands.DeleteDocument
{
    public record DeleteCaseDocumentCommand(Guid Id):IRequest<string>;

}
