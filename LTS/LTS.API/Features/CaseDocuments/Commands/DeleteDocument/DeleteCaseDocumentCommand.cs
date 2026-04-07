using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseDocuments.Commands.DeleteDocument
{
    public record DeleteCaseDocumentCommand(Guid Id):IRequest<ApiResponse<string>>;

}
