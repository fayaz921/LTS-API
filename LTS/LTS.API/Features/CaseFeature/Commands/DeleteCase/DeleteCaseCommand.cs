using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseFeature.Commands.DeleteCase
{
    public record DeleteCaseCommand(Guid CaseId) : IRequest<ApiResponse<string>>;
}