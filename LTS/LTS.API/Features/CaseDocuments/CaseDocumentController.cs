using LTS.API.Features.CaseDocuments.Commands.DeleteDocument;
using LTS.API.Features.CaseDocuments.Commands.UploadDocument;
using LTS.API.Features.CaseDocuments.Queries.GetDocumentByCase;
using LTS.API.Features.CaseDocuments.Queries.GetDocumentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.CaseDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseDocumentController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadDocument([FromForm] UploadCaseDocumentCommand command)
        {
            var result = await mediator.Send(command);
            return StatusCode((int)result.Status, result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var result = await mediator.Send(new DeleteCaseDocumentCommand(id));
            return StatusCode((int)result.Status, result);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            var result = await mediator.Send(new GetDocumentByIdQuery(id));
            return StatusCode((int)result.Status, result);
        }
        [HttpGet("case/{caseId:guid}")]
        public async Task<IActionResult> GetDocumentsByCaseId(Guid caseId)
        {
            var result = await mediator.Send(new GetDocumentByCaseIdQuery(caseId));
            return StatusCode((int)result.Status, result);
        }
    }
}
